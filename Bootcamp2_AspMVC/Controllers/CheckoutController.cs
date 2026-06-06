using Bootcamp2_AspMVC.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp2_AspMVC.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICheckoutService _svc;

        public CheckoutController(ICheckoutService svc)
        {
            _svc = svc;
        }

        // إنشاء الطلب من السلة
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] int customerId)
        {
            var (ok, msg, orderId) = await _svc.CreateOrderFromCartAsync(customerId, TimeSpan.FromMinutes(30));
            if (!ok) return BadRequest(new { message = msg });
            return Ok(new { message = msg, orderId });
        }














        // محاكاة الدفع الناجح
        [HttpPost("pay/{orderId:int}")]
        public async Task<IActionResult> Pay(int orderId)
        {
            var affected = await _svc.MarkOrderPaidAsync(orderId);
            if (affected == 0) return BadRequest(new { message = "تعذّر إتمام الدفع (طلب غير صالح أو انتهت صلاحية الحجز)." });
            return Ok(new { message = "تم الدفع وتثبيت المخزون بنجاح." });
        }

        // إلغاء يدوي
        [HttpPost("cancel/{orderId:int}")]
        public async Task<IActionResult> Cancel(int orderId)
        {
            var affected = await _svc.CancelOrExpireOrderAsync(orderId);
            if (affected == 0) return BadRequest(new { message = "تعذّر الإلغاء." });
            return Ok(new { message = "تم إلغاء الطلب وإرجاع الحجز." });
        }
    }

}
