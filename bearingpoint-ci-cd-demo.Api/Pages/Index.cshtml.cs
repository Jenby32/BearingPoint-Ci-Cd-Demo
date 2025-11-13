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
                    "Ich bin Kevin Kozar, Software Engineer mit einer Leidenschaft für Automatisierung, "
                    + "Datenverarbeitung und moderne IT-Infrastrukturen. Meine Laufbahn begann mit einer Lehre "
                    + "zum IT-Systemtechniker bei BearingPoint, wo ich erste Erfahrung mit Netzwerken und Systemadministration sammelte."),

                new("Was mache ich aktuell?",
                    "Derzeit arbeite ich bei der AVL List GmbH als Software and Safety Engineer. "
                    + "Dort entwickle und analysiere ich sicherheitsrelevante Softwaresysteme, "
                    + "erstelle technische Reports und werte Testergebnisse strukturiert aus."),

                new("Welche Technologien und Werkzeuge setze ich ein?",
                    "Ich arbeite regelmäßig mit C#, Python, Bash / PowerShell und SQL. "
                    + "Im CI/CD-Umfeld nutze ich Azure DevOps und GitHub Actions, "
                    + "und habe Erfahrung mit Containerisierung (Docker) sowie Cloud-Architekturen auf Azure und AWS."),

                new("Was interessiert mich besonders?",
                    "Mich faszinieren moderne Software-Automatisierung, die Integration intelligenter Systeme "
                    + "und die Verbindung von Entwicklung und Betrieb. "
                    + "Ich arbeite gerne an Projekten, in denen Technik Prozesse effizienter und sicherer macht."),

                new("Was zeichnet mich aus?",
                    "Ich bringe technisches Verständnis, Eigeninitiative und eine strukturierte Arbeitsweise mit. "
                    + "Ich analysiere gerne komplexe Probleme, finde pragmatische Lösungen "
                    + "und bleibe auch in stressigen Situationen ruhig und fokussiert."),

                new("Warum bewerbe ich mich bei BearingPoint?",
                    "Ich kenne die Unternehmenskultur bereits aus meiner Ausbildungszeit und schätze den "
                    + "innovativen, praxisnahen Ansatz von BearingPoint. "
                    + "Mit meinem gewachsenen Know-how in Softwareentwicklung und Automatisierung "
                    + "möchte ich nun erneut zum Erfolg des Unternehmens beitragen."),

                new("Wie kann man mich erreichen?",
                    "E-Mail: kevinkozar2005@gmail.com · Tel.: +43 676 4277312 · "
                    + "LinkedIn: linkedin.com/in/kevin-kozar")
            };
        }
    }
}
