using EPiServer.Shell;

namespace Gro.Core.ContentTypes.Blocks.BootstrapContainers
{
    /// <summary>
    /// Indicator interface for a block that is available in a one-column container.
    /// Any block with this interface will be available to be added to any ContentArea with IOneColumnBlock in its allowed types.
    /// </summary>
    public interface IOneColumnContainer
    {
    }

    /// <summary>
    /// This descriptor class makes it so that any block with this UIDescriptor T (type) can be used in any ContentArea in its 'AllowedTypes' attribute.
    /// </summary>
    [UIDescriptorRegistration]
    public class OneColumnContainerDescriptor : UIDescriptor<IOneColumnContainer>
    {
    }
}
