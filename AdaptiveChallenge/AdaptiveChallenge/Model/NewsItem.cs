using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveChallenge.Model
{
    public class NewsItem
    {
        public int Id { get; set; }
        public String Category { get; set; }
        public String Headline { get; set; }
        public String Subhead { get; set; }
        public String DateLine { get; set; }
        public String Image { get; set; }
    }

    class Factory {
        public static List<NewsItem> getNewsFinancial()
        {
            var items = new List<NewsItem>();

            items.Add(new NewsItem() { Id = 1, Category = "Financial", Headline = "Lorem Ipsum", Subhead = "doro sit amet", DateLine = "Nunc tristique nec", Image = "Assets/Financial1.png" });
            items.Add(new NewsItem() { Id = 2, Category = "Financial", Headline = "Etiam ac felis viverra", Subhead = "vulputate nisl ac, aliquet nisi", DateLine = "tortor porttitor, eu fermentum ante congue", Image = "Assets/Financial2.png" });
            items.Add(new NewsItem() { Id = 3, Category = "Financial", Headline = "Integer sed turpis erat", Subhead = "Sed quis hendrerit lorem, quis interdum dolor", DateLine = "in viverra metus facilisis sed", Image = "Assets/Financial3.png" });
            items.Add(new NewsItem() { Id = 4, Category = "Financial", Headline = "Proin sem neque", Subhead = "aliquet quis ipsum tincidunt", DateLine = "Integer eleifend", Image = "Assets/Financial4.png" });
            items.Add(new NewsItem() { Id = 5, Category = "Financial", Headline = "Mauris bibendum non leo vitae tempor", Subhead = "In nisl tortor, eleifend sed ipsum eget", DateLine = "Curabitur dictum augue vitae elementum ultrices", Image = "Assets/Financial5.png" });

            return items;
        }

        public static List<NewsItem> getNewsFood()
        {
            var items = new List<NewsItem>();

            items.Add(new NewsItem() { Id = 6, Category = "Food", Headline = "Lorem ipsum", Subhead = "dolor sit amet", DateLine = "Nunc tristique nec", Image = "Assets/Food1.png" });
            items.Add(new NewsItem() { Id = 7, Category = "Food", Headline = "Etiam ac felis viverra", Subhead = "vulputate nisl ac, aliquet nisi", DateLine = "tortor porttitor, eu fermentum ante congue", Image = "Assets/Food2.png" });
            items.Add(new NewsItem() { Id = 8, Category = "Food", Headline = "Integer sed turpis erat", Subhead = "Sed quis hendrerit lorem, quis interdum dolor", DateLine = "in viverra metus facilisis sed", Image = "Assets/Food3.png" });
            items.Add(new NewsItem() { Id = 9, Category = "Food", Headline = "Proin sem neque", Subhead = "aliquet quis ipsum tincidunt", DateLine = "Integer eleifend", Image = "Assets/Food4.png" });
            items.Add(new NewsItem() { Id = 10, Category = "Food", Headline = "Mauris bibendum non leo vitae tempor", Subhead = "In nisl tortor, eleifend sed ipsum eget", DateLine = "Curabitur dictum augue vitae elementum ultrices", Image = "Assets/Food5.png" });

            return items;
        }
    }
}
