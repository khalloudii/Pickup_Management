using DataAccess;
using DataAccess.Entities;
using Newtonsoft.Json;
using Pickup_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Pickup_Management.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        
        public OrderController()
        {
            _unitOfWork = new UnitOfWork();
        }

        // GET: Order
        public async Task<ActionResult> Index()
        {
            List<Order> list = new List<Order>();
            var userName = User.Identity.Name;
            list = await GetOrdersOfCurrentUser(userName);

            return View(list);
        }

        private async Task<List<Order>> GetOrdersOfCurrentUser(string userName)
        {
            return await _unitOfWork.OrderRepository.GetOrdersByUserNameAsync(userName); 
        }

        public ActionResult Add()
        {
            OrderViewModel model = new OrderViewModel();
            var items = new List<SelectListItem>();
            items = _unitOfWork.ItemRepository.ListAll()
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

            model.Items = items;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO
                // check if order is successfully created
                Order order = await SaveOrder(model);
                SaveOrderItems(order, model.SelectedItemsWithNumbers);

                await _unitOfWork.Complete();
            }

            return RedirectToAction("Index", "Order");
        }

        private void SaveOrderItems(Order order, string selectedItemsWithNumbers)
        {
            List<OrderItemViewModel> itemsSelectd = GetSelectedItems(selectedItemsWithNumbers);
            
            foreach (var model in itemsSelectd)
            {
                OrderItem item = new OrderItem();

                item.ItemID = model.ItemID;
                item.Quantity = model.Quantity;
                item.Order = order;

                _unitOfWork.OrderItemRepository.Add(item);
            }
        }

        private List<OrderItemViewModel> GetSelectedItems(string selectedItemsWithNumbers)
        {
            List<OrderItemViewModel> items = new List<OrderItemViewModel>();

            string[] itemWithNumberList = selectedItemsWithNumbers.TrimEnd(',').Split(',');
            foreach (string itemWithNumber in itemWithNumberList)
            {
                OrderItemViewModel model = new OrderItemViewModel();
                model.ItemID = Convert.ToInt32(itemWithNumber.Split('_')[0]);
                model.Quantity = Convert.ToInt32(itemWithNumber.Split('_')[1]);

                items.Add(model);
            }

            return items;
        }

        private async Task<Order> SaveOrder(OrderViewModel model)
        {
            Order order = new Order();
            order.Latitude = model.Latitude;
            order.Longitude = model.Longitude;

            order.Address1 = model.Address1;
            order.Address2 = model.Address2;
            order.Street = model.Street;
            order.District = model.District;
            order.PostCode = model.PostCode;
            order.City = model.City;
            order.FullName = model.FullName;
            order.Phone = model.Phone;
            order.Email = model.Email;

            order.TotalAmount = 20000; // 200 SAR

            order.PaymentLink = await GetPaymentLink();
            order.IsPaid = false;

            order.UserName = User.Identity.Name;

            // save
            _unitOfWork.OrderRepository.Add(order);

            return order;
        }

        private async Task<string> GetPaymentLink()
        {
            // first create invoice
            InvoiceViewModel invoice = await CreateInvoice();

            return invoice.url;
            //"{\"id\":\"a42fb27d-c03e-4c2c-8995-6908e163056e\",\"status\":\"initiated\",\"amount\":48000,\"currency\":\"SAR\",\"description\":\"333\",\"amount_format\":\"480.00 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/a42fb27d-c03e-4c2c-8995-6908e163056e?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-02-27T16:00:46.636Z\",\"updated_at\":\"2024-02-27T16:00:46.636Z\",\"back_url\":null,\"success_url\":null,\"metadata\":null,\"payments\":[]}"
            //"{\"id\":\"a42fb27d-c03e-4c2c-8995-6908e163056e\",\"status\":\"initiated\",\"amount\":48000,\"currency\":\"SAR\",\"description\":\"333\",\"amount_format\":\"480.00 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/a42fb27d-c03e-4c2c-8995-6908e163056e?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-02-27T16:00:46.636Z\",\"updated_at\":\"2024-02-27T16:00:46.636Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":null},{\"id\":\"0084a8d8-1aa3-4f87-ac99-3b206c9e3967\",\"status\":\"initiated\",\"amount\":10000,\"currency\":\"SAR\",\"description\":\"123\",\"amount_format\":\"100.00 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/0084a8d8-1aa3-4f87-ac99-3b206c9e3967?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-02-27T15:56:40.780Z\",\"updated_at\":\"2024-02-27T15:56:40.780Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":null},{\"id\":\"e1ad595a-f956-4914-8aab-440d01816cda\",\"status\":\"initiated\",\"amount\":1099,\"currency\":\"SAR\",\"description\":\"فاتورة لمحمد أحمد مقابل خدمة تصميم وانتاج ملف عرض \",\"amount_format\":\"10.99 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/e1ad595a-f956-4914-8aab-440d01816cda?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-01-20T20:31:03.137Z\",\"updated_at\":\"2024-01-20T20:31:03.137Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":{\"test\":\"101\",\"تجريبي\":\"واحد\"}},{\"id\":\"535ed563-f44f-4ec8-8bc4-a18c12c6554b\",\"status\":\"refunded\",\"amount\":33444,\"currency\":\"SAR\",\"description\":\"ملاحظة: التفاصيل ستظهر لدى العميل ملاحظة: التفاصيل ستظهر لدى العميل ملاحظة: التفاصيل ستظهر لدى العميل 123\",\"amount_format\":\"334.44 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/535ed563-f44f-4ec8-8bc4-a18c12c6554b?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-01-20T19:57:59.923Z\",\"updated_at\":\"2024-01-21T20:47:33.011Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":null},{\"id\":\"6fcaf1df-9bf4-4ede-969b-2e1c994357ab\",\"status\":\"paid\",\"amount\":9088,\"currency\":\"SAR\",\"description\":\"a2\",\"amount_format\":\"90.88 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/6fcaf1df-9bf4-4ede-969b-2e1c994357ab?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-01-20T19:27:17.807Z\",\"updated_at\":\"2024-01-20T19:28:34.220Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":\"351613f8-9875-48ab-b3fe-8149b7a8f224\",\"paid_at\":\"2024-01-20T19:28:31.165Z\",\"metadata\":null},{\"id\":\"51eea859-851a-4ebc-9113-ab4592e9fe1e\",\"status\":\"initiated\",\"amount\":1234,\"currency\":\"SAR\",\"description\":\"desc 1234\",\"amount_format\":\"12.34 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/51eea859-851a-4ebc-9113-ab4592e9fe1e?lang=en\",\"callback_url\":\"\",\"expired_at\":null,\"created_at\":\"2024-01-20T11:56:06.309Z\",\"updated_at\":\"2024-01-20T11:56:06.309Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":null}],\"meta\":{\"current_page\":1,\"next_page\":null,\"prev_page\":null,\"total_pages\":1,\"total_count\":6}}"
            //"{\"invoices\":[{\"id\":\"a42fb27d-c03e-4c2c-8995-6908e163056e\",\"status\":\"initiated\",\"amount\":48000,\"currency\":\"SAR\",\"description\":\"333\",\"amount_format\":\"480.00 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/a42fb27d-c03e-4c2c-8995-6908e163056e?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-02-27T16:00:46.636Z\",\"updated_at\":\"2024-02-27T16:00:46.636Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":null},{\"id\":\"0084a8d8-1aa3-4f87-ac99-3b206c9e3967\",\"status\":\"initiated\",\"amount\":10000,\"currency\":\"SAR\",\"description\":\"123\",\"amount_format\":\"100.00 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/0084a8d8-1aa3-4f87-ac99-3b206c9e3967?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-02-27T15:56:40.780Z\",\"updated_at\":\"2024-02-27T15:56:40.780Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":null},{\"id\":\"e1ad595a-f956-4914-8aab-440d01816cda\",\"status\":\"initiated\",\"amount\":1099,\"currency\":\"SAR\",\"description\":\"فاتورة لمحمد أحمد مقابل خدمة تصميم وانتاج ملف عرض \",\"amount_format\":\"10.99 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/e1ad595a-f956-4914-8aab-440d01816cda?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-01-20T20:31:03.137Z\",\"updated_at\":\"2024-01-20T20:31:03.137Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":{\"test\":\"101\",\"تجريبي\":\"واحد\"}},{\"id\":\"535ed563-f44f-4ec8-8bc4-a18c12c6554b\",\"status\":\"refunded\",\"amount\":33444,\"currency\":\"SAR\",\"description\":\"ملاحظة: التفاصيل ستظهر لدى العميل ملاحظة: التفاصيل ستظهر لدى العميل ملاحظة: التفاصيل ستظهر لدى العميل 123\",\"amount_format\":\"334.44 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/535ed563-f44f-4ec8-8bc4-a18c12c6554b?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-01-20T19:57:59.923Z\",\"updated_at\":\"2024-01-21T20:47:33.011Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":null},{\"id\":\"6fcaf1df-9bf4-4ede-969b-2e1c994357ab\",\"status\":\"paid\",\"amount\":9088,\"currency\":\"SAR\",\"description\":\"a2\",\"amount_format\":\"90.88 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/6fcaf1df-9bf4-4ede-969b-2e1c994357ab?lang=en\",\"callback_url\":\"https://localhost:44386/invoice/\",\"expired_at\":null,\"created_at\":\"2024-01-20T19:27:17.807Z\",\"updated_at\":\"2024-01-20T19:28:34.220Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":\"351613f8-9875-48ab-b3fe-8149b7a8f224\",\"paid_at\":\"2024-01-20T19:28:31.165Z\",\"metadata\":null},{\"id\":\"51eea859-851a-4ebc-9113-ab4592e9fe1e\",\"status\":\"initiated\",\"amount\":1234,\"currency\":\"SAR\",\"description\":\"desc 1234\",\"amount_format\":\"12.34 SAR\",\"url\":\"https://checkout.moyasar.com/invoices/51eea859-851a-4ebc-9113-ab4592e9fe1e?lang=en\",\"callback_url\":\"\",\"expired_at\":null,\"created_at\":\"2024-01-20T11:56:06.309Z\",\"updated_at\":\"2024-01-20T11:56:06.309Z\",\"back_url\":null,\"success_url\":null,\"payment_id\":null,\"paid_at\":null,\"metadata\":null}],\"meta\":{\"current_page\":1,\"next_page\":null,\"prev_page\":null,\"total_pages\":1,\"total_count\":6}}"
        }

        /// <summary>
        /// Moyasar Payment API
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<InvoiceViewModel> CreateInvoice()
        {
            InvoiceViewModel model = new InvoiceViewModel();

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    string secret_key = "sk_test_zjNateVMbSx9VPqF46tx3v2kivpdggVhW67MLBvh:";
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(secret_key);
                    var encode = Convert.ToBase64String(plainTextBytes);

                    client.BaseAddress = new Uri("https://api.moyasar.com/v1/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Basic " + encode);

                    var content = new FormUrlEncodedContent(new[]
                    {
                     new KeyValuePair<string, string>("amount", "20000"),
                     new KeyValuePair<string, string>("currency", "SAR"),
                     new KeyValuePair<string, string>("description", "description here"),
                     new KeyValuePair<string, string>("callback_url", "https://miniproject.aqaraljanoob.com/"),
                });

                    HttpResponseMessage response = await client.PostAsync("invoices", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = response.Content.ReadAsStringAsync().Result;//.Replace("\"", string.Empty);
                        model = JsonConvert.DeserializeObject<InvoiceViewModel>(jsonData);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: write in txt file
                var test = ex.Message;
            }

            return model;
        }

        public ActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Form(OrderViewModel model)
        {
            return View();
        }
    }
}