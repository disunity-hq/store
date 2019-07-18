using System;
using System.IO;
using System.Linq;

using BindingAttributes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Slugify;

using Tracery;


namespace Disunity.Store.Shared.Data.Services
{

    public class TraceryUnparser
    {

        [Factory]
        public static Func<string, Unparser> UnparserFactory(IServiceProvider di)
        {
            var config = di.GetRequiredService<IConfiguration>();
            var thesaurus = di.GetRequiredService<IThesaurus>();
            var slugifier = di.GetRequiredService<ISlugifier>();

            string CapitalizeEach(string s)
            {
                var parts = s.Split(null)
                             .Where(p => p.Count() > 0)
                             .Select(p => char.ToUpper(p[0]) + p.Substring(1));

                return String.Join(" ", parts);
            }

            return filename =>
            {
                var grammarPath = config.GetValue<string>("Database:TraceryGrammarPath");
                var fullPath = Path.Combine(grammarPath, filename);
                var grammar = Grammar.FromFile(fullPath);
                var unparser = new Unparser(grammar);

                unparser.Modifiers["capitalizeEach"] = CapitalizeEach;
                unparser.Modifiers["alt"] = thesaurus.For;
                unparser.Modifiers["slug"] = slugifier.Slugify;

                return unparser;
            };
        }
    }
}