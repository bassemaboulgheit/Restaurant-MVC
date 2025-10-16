using System.Text;
using Applications.DTos;
using Applications.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Restaurant.TaqHelpers
{
    [HtmlTargetElement("item-card")]
    public class CardTagHelper : TagHelper
    {
        public ItemsDto p { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var sb = new StringBuilder();
            sb.AppendLine("<div class='card' style='width: 18rem; border: 1px solid #ddd; border-radius: 8px; padding: 16px; margin: 16px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>");

            sb.AppendLine($"<h5 class='card-title' style='font-size: 1.25rem; font-weight: bold;'>{p.Name}</h5>");

            sb.AppendLine($"<p class='card-text' style='color: #555;'><strong>{p.Description}</strong></p>");

            sb.AppendLine($"<p class='card-text' style='color: #28a745; font-weight: bold;'><strong>${p.Price:F2}</strong></p>");

            var stockStatus = p.Quantity > 0 ? "In Stock" : "Out of Stock";
            var stockColor = p.Quantity > 0 ? "green" : "red";
            sb.AppendLine($"<p class='card-text' style='color: {stockColor};'><strong>{stockStatus}</strong></p>");

            sb.AppendLine($"<p class='card-text' style='font-size: 0.9rem; color: #777;'><strong>Category: {p.Category.Name}</strong></p>");

            sb.AppendLine($"<p class='card-text' style='font-size: 0.9rem; color: #777;'><strong>Image URL: {p.ImageUrl}</strong></p>");

            sb.AppendLine("</div>");

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
