using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreApp.Pages
{
    public class DemoModel : PageModel
    {
        public String? FullName => HttpContext.Session.GetString("name") ?? "User";

        public void OnGet()
        {
        }

        public void OnPost([FromForm(Name ="name")] string name) 
        {
            //FullName = name;
            HttpContext.Session.SetString("name", name); // Sessionda ismi saklýyoruz
        }
    }
}
