using System.Collections.Generic;
using EPiServer.Core.PropertySettings;
using EPiServer.Editor.TinyMCE;
using EPiServer.ServiceLocation;

namespace Gro.Core.ContentTypes.Business.CustomSettings
{
    [ServiceConfiguration(ServiceType = typeof(PropertySettings))]
    public class CustomTinyMceSettings : PropertySettings<TinyMCESettings>
    {
        public CustomTinyMceSettings()
        {
            IsDefault = true;
            DisplayName = "Custom settings for editor";
            Description = "Custom settings as defined in code.";
        }

        public override TinyMCESettings GetPropertySettings()
        {
            var settings = new TinyMCESettings();
            var firstToolbar = new ToolbarRow(new List<string>()
            {
                TinyMCEButtons.EPiServerLink,
                TinyMCEButtons.Unlink,
                TinyMCEButtons.Separator,
                TinyMCEButtons.Image,
                TinyMCEButtons.EPiServerImageEditor,
                TinyMCEButtons.Media,
                TinyMCEButtons.EPiServerPersonalizedContent,
                TinyMCEButtons.Separator,
                TinyMCEButtons.Cut,
                TinyMCEButtons.Copy,
                TinyMCEButtons.Paste,
                TinyMCEButtons.PasteText,
                TinyMCEButtons.PasteWord,
                TinyMCEButtons.Separator,
                //TinyMCEButtons.TableButtons.Controls,
            });
            var secondToolbar = new ToolbarRow(new List<string>()
            {
                TinyMCEButtons.Bold,
                TinyMCEButtons.Italic,
                TinyMCEButtons.Separator,
                TinyMCEButtons.NumericList,
                TinyMCEButtons.BulletedList,
                TinyMCEButtons.Separator,
                TinyMCEButtons.StyleSelect,
                TinyMCEButtons.Undo,
                TinyMCEButtons.Redo,
                TinyMCEButtons.Separator,
                TinyMCEButtons.Search,
                TinyMCEButtons.Separator,
                TinyMCEButtons.Code,
                TinyMCEButtons.Fullscreen
            });
            settings.ToolbarRows.Add(firstToolbar);
            settings.ToolbarRows.Add(secondToolbar);
            return settings;
        }

        public override System.Guid ID => new System.Guid("34B7565F-503F-4435-AF90-58A7D5C03D28");
    }
}
