using System.Collections.Generic;
using System.IO;
using Xunit;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Xunit.Abstractions;
using Disunity.Store.Shared.Archive;


namespace Disunity.Store.Tests {

    public class ManifestTests {

        private readonly ITestOutputHelper log;

        public ManifestTests(ITestOutputHelper log) {
            this.log = log;
        }

        public bool RangesAreEqual(Dictionary<string, VersionRange> first,
                                   Dictionary<string, VersionRange> second) {
            if (first.Count != second.Count) {
                log.WriteLine($"Count is different: {first.Count} vs {second.Count}");
                return false;
            }
            foreach (var (key, value) in first) {
                if (!second.TryGetValue(key, out var range)) {
                    log.WriteLine($"Missing key: {key}");
                    return false;
                }

                if (!range.Equals(value)) {
                    log.WriteLine($"Versions are not equal: {value} vs {range}");
                    return false;
                }
            }

            return true;
        }

        [Fact]
        public void CanDeserialize() {
            var json = File.ReadAllText("data/manifest.json");
            var manifest = Manifest.FromJson(json);
            foreach (var key in manifest.Targets.Keys) {
                var target = manifest.Targets[key];
                log.WriteLine($"Dependency: {key} - {target.MinVersion} to {target.MaxVersion}");
            }

            Assert.Equal("fakeuser", manifest.OrgID);
            Assert.Equal("fake-mod", manifest.ModID);
            Assert.Equal("0.0.1", manifest.Version);
            Assert.Equal("Fake Mod", manifest.Name);
            Assert.Equal("http://github.com/disunity-hq/fake-mod", manifest.Url);
            Assert.Equal("An imaginary Disunity mod", manifest.Description);
            Assert.Equal(14, manifest.ContentTypes);

            Assert.True(RangesAreEqual(new Dictionary<string, VersionRange> {
                {"risk-of-rain-2", new VersionRange("3830295", "3830297")}
            }, manifest.Targets));
            Assert.True(RangesAreEqual(new Dictionary<string, VersionRange> {
                {"foo/bar", new VersionRange("2.0.0")}
            }, manifest.Dependencies));
            
            Assert.Equal(new[] { "FakePreload.dll"}, manifest.PreloadAssemblies);
            Assert.Equal(new[] { "FakeRuntime.dll"}, manifest.RuntimeAssemblies);
            
            Assert.Equal("FakePreload", manifest.PreloadAssembly);
            Assert.Equal("FakePreload", manifest.PreloadClass);
            Assert.Equal("FakeRuntime", manifest.RuntimeAssembly);
            Assert.Equal("FakeRuntime", manifest.RuntimeClass);
        }

    }

}