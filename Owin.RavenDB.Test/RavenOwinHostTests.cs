using Microsoft.Owin.Hosting;
using Owin;
using Raven.Client.Document;
using Raven.Database.Server;
using Xunit;

namespace Owin.RavenDB.Test
{
    public class RavenOwinHostTests
    {
        const string Url = "http://localhost:80/";

        [Fact]
        public void Can_save_and_retrieve_document()
        {
            using (WebApp.Start<Startup>(Url))
            using (var store = new DocumentStore {Url = Url}.Initialize())
            {
                using (var session = store.OpenSession())
                {
                    var doc = new Doc { Id = "Id" };
                    session.Store(doc);
                    session.SaveChanges();
                }

                using (var session = store.OpenSession())
                {
                    var doc = session.Load<Doc>("Id");
                    Assert.NotNull(doc);
                }
            }
        }
        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseRavenDB();
            }
        }

        public class Doc
        {
            public string Id { get; set; }
        }
    }
}
