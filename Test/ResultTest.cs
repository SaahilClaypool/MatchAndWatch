using System;

using Core;

using Extensions;

using Xunit;

namespace Test {
    public class ResultTest {
        [Fact]
        public void OkayTest() {
            var res = Result<string>.Ok("Hello World");
            Assert.True(res.IsOk(out string val));
            Assert.Equal("Hello World", val);
        }

        [Fact]
        public void FailureTest() {
            var res = Result<string>.Fail("Hello World");
            Assert.False(res.IsOk(out string _, out string err));
            Assert.Equal("Hello World", err);
        }

        [Fact]
        public void OnThingTest() {
            var res = Result<string>.Fail("Hello World");
            Assert.Equal("Default", res.DefaultValue("Default"));
            res = Result<string>.Ok("");
            Assert.Equal(
              "Map",
              res.Map(val => "Map").DefaultValue("fail")
            );
            Assert.Equal(
              "Val",
              res.Map(val => "Val").Value!
            );
        }
    }
}
