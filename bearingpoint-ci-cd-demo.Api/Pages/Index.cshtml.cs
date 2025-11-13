using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace bearingpoint_ci_cd_demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public record FaqItem(string Question, string Answer);

        public List<FaqItem> Faqs { get; private set; } = new();

        public void OnGet()
        {
            Faqs = new()
            {
                new("Wer bin ich?",
                    "Ich bin Kevin, Softwareentwickler mit Fokus auf .NET, Azure und CI/CD."),
                new("Warum dieses Projekt?",
                    "Dieses Projekt ist meine Bewerbungs-Demo. Es zeigt CI/CD, Azure-Deployment, Healthchecks und ein eigenes Frontend."),
                new("Welche Technologien nutze ich?",
                    "ASP.NET Core, C#, Razor Pages, Azure App Service, GitHub Actions und Ansible."),
                new("Was zeichnet mich aus?",
                    "Ich arbeite lösungsorientiert, strukturiert und automatisiere gern wiederkehrende Abläufe."),
                new("Wie kann man mich erreichen?",
                    "E-Mail: kevinkozar2005@gmail.com · LinkedIn: https://www.linkedin.com/in/kevin-kozar-5a677630b/")
            };
        }
    }
}
