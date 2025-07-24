using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DevIO.App.Extensions
{
    public class EmailTagHelper : TagHelper
    {
        public string EmailDomain { get; set; } = "gmail.com";
        public string EmailName { get; set; } = "m.barceloslima";
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var target = EmailName+ "@" + EmailDomain;
            output.Attributes.SetAttribute("href", "mailto:" + target);
            output.Content.SetContent(target);
        }
    }
}
