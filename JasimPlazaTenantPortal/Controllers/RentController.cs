using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jasim_Plaza_Tenant_Portal.EF.Tables;
using Jasim_Plaza_Tenant_Portal.Services;

public class RentController : Controller
{
    private readonly JasimPlazaDbContext db;
    private readonly IBkashPaymentService bkashPaymentService;

    public RentController(JasimPlazaDbContext db, IBkashPaymentService bkashPaymentService)
    {
        this.db = db;
        this.bkashPaymentService = bkashPaymentService;
    }

    public IActionResult Index(int? month, int? year, string? status, string? shopName)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        IQueryable<RentRecord> query = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner);

        if (!IsAdmin(currentUser))
        {
            query = query.Where(r => r.Shop.OwnerId == currentUser.Id);
        }

        if (month.HasValue)
        {
            query = query.Where(r => r.Month == month.Value);
        }

        if (year.HasValue)
        {
            query = query.Where(r => r.Year == year.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(r => r.Status == status);
        }

        if (IsAdmin(currentUser) && !string.IsNullOrWhiteSpace(shopName))
        {
            query = query.Where(r => r.Shop.ShopName.Contains(shopName));
        }

        var records = query
            .OrderByDescending(r => r.Year)
            .ThenByDescending(r => r.Month)
            .ThenBy(r => r.Shop.ShopName)
            .ToList();

        ViewBag.CurrentUser = currentUser;
        ViewBag.IsAdmin = IsAdmin(currentUser);
        ViewBag.Month = month;
        ViewBag.Year = year;
        ViewBag.Status = status;
        ViewBag.ShopName = shopName;

        return View(records);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can add rent records.";
            return RedirectToAction("Index");
        }

        var shops = db.Shops
            .Include(s => s.Owner)
            .OrderBy(s => s.ShopName)
            .ToList();

        ViewBag.CurrentUser = currentUser;
        return View(shops);
    }

    [HttpPost]
    public IActionResult Create(RentRecord rec)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can add rent records.";
            return RedirectToAction("Index");
        }

        var shop = db.Shops.Include(s => s.Owner).FirstOrDefault(s => s.Id == rec.ShopId);
        if (shop == null)
        {
            TempData["msg"] = "Selected shop does not exist.";
            return RedirectToAction("Create");
        }

        var duplicateRecordExists = db.RentRecords.Any(r => r.ShopId == rec.ShopId && r.Month == rec.Month && r.Year == rec.Year);
        if (duplicateRecordExists)
        {
            TempData["msg"] = "A record for this shop and month already exists.";
            return RedirectToAction("Create");
        }

        if (rec.Month is < 1 or > 12)
        {
            TempData["msg"] = "Month must be between 1 and 12.";
            return RedirectToAction("Create");
        }

        if (!new[] { "Paid", "Pending", "Due" }.Contains(rec.Status))
        {
            TempData["msg"] = "Invalid status value.";
            return RedirectToAction("Create");
        }

        if (rec.Amount <= 0)
        {
            TempData["msg"] = "Amount must be greater than 0.";
            return RedirectToAction("Create");
        }

        rec.PaymentDate = rec.Status == "Paid" ? DateOnly.FromDateTime(DateTime.Today) : null;

        try
        {
            db.Add(rec);
            db.SaveChanges();
            TempData["msg"] = "Rent record created successfully.";
            return RedirectToAction("Index");
        }
        catch
        {
            TempData["msg"] = "An unknown error occurred while creating rent record.";
            return RedirectToAction("Create");
        }

    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can edit rent records.";
            return RedirectToAction("Index");
        }

        var record = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner)
            .FirstOrDefault(r => r.Id == id);

        if (record == null)
        {
            return NotFound();
        }

        ViewBag.Shops = db.Shops.Include(s => s.Owner).OrderBy(s => s.ShopName).ToList();
        return View(record);
    }

    [HttpPost]
    public IActionResult Edit(RentRecord rec)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can edit rent records.";
            return RedirectToAction("Index");
        }

        var record = db.RentRecords.FirstOrDefault(r => r.Id == rec.Id);
        if (record == null)
        {
            return NotFound();
        }

        if (rec.Month is < 1 or > 12 || rec.Amount <= 0)
        {
            TempData["msg"] = "Please provide valid values.";
            return RedirectToAction("Edit", new { id = rec.Id });
        }

        if (!new[] { "Paid", "Pending", "Due" }.Contains(rec.Status))
        {
            TempData["msg"] = "Invalid status value.";
            return RedirectToAction("Edit", new { id = rec.Id });
        }

        var duplicateRecordExists = db.RentRecords.Any(r => r.Id != rec.Id && r.ShopId == rec.ShopId && r.Month == rec.Month && r.Year == rec.Year);
        if (duplicateRecordExists)
        {
            TempData["msg"] = "Another record already exists for this shop and month.";
            return RedirectToAction("Edit", new { id = rec.Id });
        }

        record.ShopId = rec.ShopId;
        record.Month = rec.Month;
        record.Year = rec.Year;
        record.Amount = rec.Amount;
        record.Status = rec.Status;
        record.PaymentDate = rec.Status == "Paid" ? (rec.PaymentDate ?? DateOnly.FromDateTime(DateTime.Today)) : null;

        db.SaveChanges();
        TempData["msg"] = "Rent record updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can delete rent records.";
            return RedirectToAction("Index");
        }

        var record = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner)
            .FirstOrDefault(r => r.Id == id);

        if (record == null)
        {
            return NotFound();
        }

        return View(record);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var currentUser = GetCurrentUser();
        if (!IsAdmin(currentUser))
        {
            TempData["msg"] = "Only Admin can delete rent records.";
            return RedirectToAction("Index");
        }

        var record = db.RentRecords.FirstOrDefault(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        db.RentRecords.Remove(record);
        db.SaveChanges();

        TempData["msg"] = "Rent record deleted successfully.";
        return RedirectToAction("Index");
    }


    public IActionResult Details(int id)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        if (!IsAdmin(currentUser))
        {
            return Forbid();
        }

        var record = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner)
            .FirstOrDefault(r => r.Id == id);

        if (record == null)
        {
            return NotFound();
        }

        ViewBag.CurrentUser = currentUser;
        return View(record);
    }

    public IActionResult DetailsByCode(string invoiceId)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        int recordId;
        int month;
        int? shopId = null;

        if (TryParseOwnerInvoiceId(invoiceId, out recordId, out month))
        {
            // Owner-safe invoice code does not expose shop id.
        }
        else if (TryParseInvoiceId(invoiceId, out recordId, out var parsedShopId, out month))
        {
            shopId = parsedShopId;
        }
        else
        {
            return NotFound();
        }

        var record = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner)
            .FirstOrDefault(r => r.Id == recordId);

        if (record == null || record.Month != month)
        {
            return NotFound();
        }

        if (shopId.HasValue && record.ShopId != shopId.Value)
        {
            return NotFound();
        }

        if (!IsAdmin(currentUser) && record.Shop.OwnerId != currentUser.Id)
        {
            return Forbid();
        }

        ViewBag.CurrentUser = currentUser;
        return View("Details", record);
    }

    public IActionResult Invoice(int id)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        if (!IsAdmin(currentUser))
        {
            return Forbid();
        }

        var record = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner)
            .FirstOrDefault(r => r.Id == id);

        if (record == null)
        {
            return NotFound();
        }

        ViewData["Title"] = "Invoice";
        return View(record);
    }

    public IActionResult InvoiceByCode(string invoiceId)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        int recordId;
        int month;
        int? shopId = null;

        if (TryParseOwnerInvoiceId(invoiceId, out recordId, out month))
        {
            // Owner-safe invoice code does not expose shop id.
        }
        else if (TryParseInvoiceId(invoiceId, out recordId, out var parsedShopId, out month))
        {
            shopId = parsedShopId;
        }
        else
        {
            return NotFound();
        }

        var record = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner)
            .FirstOrDefault(r => r.Id == recordId);

        if (record == null || record.Month != month)
        {
            return NotFound();
        }

        if (shopId.HasValue && record.ShopId != shopId.Value)
        {
            return NotFound();
        }

        if (!IsAdmin(currentUser) && record.Shop.OwnerId != currentUser.Id)
        {
            return Forbid();
        }

        ViewData["Title"] = "Invoice";
        return View("Invoice", record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PayWithBkash(string invoiceId)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        if (IsAdmin(currentUser))
        {
            return Forbid();
        }

        if (!TryParseOwnerInvoiceId(invoiceId, out var recordId, out var month))
        {
            TempData["msg"] = "Invalid invoice id.";
            return RedirectToAction("Index");
        }

        var record = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner)
            .FirstOrDefault(r => r.Id == recordId && r.Month == month);

        if (record == null)
        {
            TempData["msg"] = "Rent record not found.";
            return RedirectToAction("Index");
        }

        if (record.Shop.OwnerId != currentUser.Id)
        {
            return Forbid();
        }

        if (string.Equals(record.Status, "Paid", StringComparison.OrdinalIgnoreCase))
        {
            TempData["msg"] = "This invoice is already paid.";
            return RedirectToAction("DetailsByCode", new { invoiceId });
        }

        var callbackUrl = Url.Action(
            action: nameof(BkashCallback),
            controller: "Rent",
            values: new { invoiceId },
            protocol: Request.Scheme);

        if (string.IsNullOrWhiteSpace(callbackUrl))
        {
            TempData["msg"] = "Unable to start bKash payment.";
            return RedirectToAction("DetailsByCode", new { invoiceId });
        }

        var createResult = await bkashPaymentService.CreatePaymentAsync(record.Amount, invoiceId, callbackUrl);
        if (!createResult.Success || string.IsNullOrWhiteSpace(createResult.RedirectUrl))
        {
            TempData["msg"] = string.IsNullOrWhiteSpace(createResult.Message)
                ? "Unable to create bKash payment right now."
                : createResult.Message;
            return RedirectToAction("DetailsByCode", new { invoiceId });
        }

        return Redirect(createResult.RedirectUrl);
    }

    [HttpGet]
    public async Task<IActionResult> BkashCallback(string invoiceId, string? paymentID, string? status)
    {
        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "User", new { returnUrl = Request.Path + Request.QueryString });
        }

        if (IsAdmin(currentUser))
        {
            return Forbid();
        }

        if (!TryParseOwnerInvoiceId(invoiceId, out var recordId, out var month))
        {
            TempData["msg"] = "Invalid invoice id received from bKash callback.";
            return RedirectToAction("Index");
        }

        var record = db.RentRecords
            .Include(r => r.Shop)
            .ThenInclude(s => s.Owner)
            .FirstOrDefault(r => r.Id == recordId && r.Month == month);

        if (record == null)
        {
            TempData["msg"] = "Rent record not found for callback.";
            return RedirectToAction("Index");
        }

        if (record.Shop.OwnerId != currentUser.Id)
        {
            return Forbid();
        }

        if (!string.Equals(status, "success", StringComparison.OrdinalIgnoreCase))
        {
            TempData["msg"] = "bKash payment was canceled or failed.";
            return RedirectToAction("DetailsByCode", new { invoiceId });
        }

        if (string.IsNullOrWhiteSpace(paymentID))
        {
            TempData["msg"] = "bKash payment id is missing in callback.";
            return RedirectToAction("DetailsByCode", new { invoiceId });
        }

        var executeResult = await bkashPaymentService.ExecutePaymentAsync(paymentID);
        if (!executeResult.Success)
        {
            TempData["msg"] = string.IsNullOrWhiteSpace(executeResult.Message)
                ? "bKash payment verification failed."
                : executeResult.Message;
            return RedirectToAction("DetailsByCode", new { invoiceId });
        }

        record.Status = "Paid";
        record.PaymentDate = DateOnly.FromDateTime(DateTime.Today);
        db.SaveChanges();

        TempData["msg"] = string.IsNullOrWhiteSpace(executeResult.TransactionId)
            ? "Payment completed via bKash."
            : $"Payment completed via bKash. Transaction: {executeResult.TransactionId}";

        return RedirectToAction("DetailsByCode", new { invoiceId });
    }

    private User? GetCurrentUser()
    {
        var sessionUserId = HttpContext.Session.GetInt32("CurrentUserId");
        return sessionUserId.HasValue
            ? db.Users.FirstOrDefault(u => u.Id == sessionUserId.Value)
            : null;
    }

    private static bool IsAdmin(User? user)
    {
        return string.Equals(user?.Role, "Admin", StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildInvoiceId(RentRecord record)
    {
        return $"INV-{record.Id}-{record.ShopId}-{record.Month:D2}";
    }

    private static string BuildOwnerInvoiceId(RentRecord record)
    {
        return $"INV-{record.Id}-{record.Month:D2}";
    }

    private static bool TryParseOwnerInvoiceId(string? invoiceId, out int recordId, out int month)
    {
        recordId = 0;
        month = 0;

        if (string.IsNullOrWhiteSpace(invoiceId))
        {
            return false;
        }

        var parts = invoiceId.Split('-', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3 || !string.Equals(parts[0], "INV", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!int.TryParse(parts[1], out recordId) || !int.TryParse(parts[2], out month))
        {
            return false;
        }

        if (recordId <= 0 || month is < 1 or > 12)
        {
            return false;
        }

        return true;
    }

    private static bool TryParseInvoiceId(string? invoiceId, out int recordId, out int shopId, out int month)
    {
        recordId = 0;
        shopId = 0;
        month = 0;

        if (string.IsNullOrWhiteSpace(invoiceId))
        {
            return false;
        }

        var parts = invoiceId.Split('-', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 4 || !string.Equals(parts[0], "INV", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!int.TryParse(parts[1], out recordId) || !int.TryParse(parts[2], out shopId) || !int.TryParse(parts[3], out month))
        {
            return false;
        }

        if (recordId <= 0 || shopId <= 0 || month is < 1 or > 12)
        {
            return false;
        }

        return true;
    }
}