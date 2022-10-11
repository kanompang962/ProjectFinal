using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectFinal.Models;
using System.Text;

namespace ProjectFinal.Controllers
{
    public class CallIssueController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        // GET: CallIssueController
        public CallIssueController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback =
                (sender, cert, chain, sslPolicyErrors) => { return true; };
        }
        public async Task<ActionResult> Index()
        {
            var issue = await GetIssue();
            return View(issue);
        }
        [HttpGet]
        public async Task<List<Issue>> GetIssue()
        {
            List<Issue> issueList = new List<Issue>();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:7116/api/Issue"))
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    issueList = JsonConvert.DeserializeObject<List<Issue>>(strJson);
                }
            }
            return issueList;
        }
        public async Task<ActionResult> Details(int id)
        {
            Issue issue = new Issue();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:7116/api/Issue/id?id=" + id))
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    issue = JsonConvert.DeserializeObject<Issue>(strJson);
                }
            }
            return View(issue);
        }



        // GET: CallIssueController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CallIssueController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Issue issue)
        {
            try
            {
                Issue sue = new Issue();
                using (var httpClient = new HttpClient(_clientHandler))
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(issue), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:7116/api/Issue", content))
                    {
                        string strJson = await response.Content.ReadAsStringAsync();
                        sue = JsonConvert.DeserializeObject<Issue>(strJson);
                        if (ModelState.IsValid)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                return View(sue);
            }
            catch
            {
                return View();
            }

        }

        // GET: CallIssueController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Issue issue = new Issue();
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:7116/api/Issue/id?id=" + id))
                {
                    string strJson = await response.Content.ReadAsStringAsync();
                    issue = JsonConvert.DeserializeObject<Issue>(strJson);
                }
            }
            return View(issue);
        }

        // POST: CallIssueController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Issue issue)
        {
            string del = "";
            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content =
                    new StringContent(JsonConvert.SerializeObject(issue), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7116/api/Issue/" + id, content))
                {
                    del = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));


        }

        // GET: CallIssueController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            string del = "";
            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7116/api/Issue/" + id))
                {
                    del = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: CallIssueController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}