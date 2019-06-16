using SlothEnterprise.ProductApplication.Applications;

namespace SlothEnterprise.ProductApplication.Products
{
    public interface IProduct
    {
        int Id { get; }

        int Visit(ISubmitApplicationVisitor visitor, ISellerApplication application);
    }
}
