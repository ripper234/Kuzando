using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace Kuzando.Common.Web
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly WindsorContainer _container;

        public WindsorControllerFactory(WindsorContainer container)
        {
            _container = container;
        }

        public virtual IController CreateController(RequestContext requestContext, string controllerName)
        {
            // todo
            // http://stackoverflow.com/questions/2613615/does-not-implement-icontrollerfactory-createcontroller-in-visual-studio-2010-rc
            return (IController)_container.Resolve(controllerName);
        }

        public override void ReleaseController(IController controller)
        {
            _container.Release(controller);
            base.ReleaseController(controller);
        }
    }

    public class AAA
    {
        public void Foo()
        {}
    }

    public class BBB : AAA
    {
        public void Foo()
        {
    }
}
