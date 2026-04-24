using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace SafeVoice.Controllers
{
    public class DonationController : Controller
    {
        private readonly IConfiguration _configuration;

        public DonationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Donation page
        public IActionResult Index()
        {
            ViewBag.PublishableKey = _configuration["Stripe:PublishableKey"];
            return View();
        }

        // Create Stripe checkout session
        [HttpPost]
        public IActionResult CreateCheckoutSession(int amount)
        {
            var domain = $"{Request.Scheme}://{Request.Host}";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            UnitAmount = amount * 100, // Stripe uses cents
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Donation to SafeVoice",
                                Description = "Help us protect children across Ireland",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = $"{domain}/Donation/Success",
                CancelUrl = $"{domain}/Donation/Cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

        public IActionResult Success() => View();
        public IActionResult Cancel() => View();
    }
}