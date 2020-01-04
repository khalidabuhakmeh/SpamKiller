using System;
using System.Threading.Tasks;
using Akismet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace SpamKiller.Pages
{
    public class Contact : PageModel
    {
        private readonly IConfiguration configuration;
        private AkismetClient Akismet { get; }

        public Contact(AkismetClient akismet, IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Akismet = akismet ?? throw new ArgumentNullException(nameof(akismet));
        }
        
        [BindProperty]
        public string Comment { get; set; }
        
        [BindProperty]
        public string Author { get; set; }
        
        public bool? IsSpam { get; set; }
        
        public async Task OnPost()
        {
            var comment = new AkismetComment
            {
                Blog = new Uri(configuration["blogUrl"]),
                CommentAuthorEmail = Author,
                CommentContent = Comment,
            };
            
            IsSpam = await Akismet.IsSpam(comment);
        }
    }
}