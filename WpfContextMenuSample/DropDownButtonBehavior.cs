using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfContextMenuSample
{
    public class DropDownButtonBehavior : Behavior<Button>
    {
        private bool isContextMenuOpen;

        /// <summary>
        /// Attach to necessary event handlers
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click), true);
            AssociatedObject.AddHandler(Button.ContextMenuOpeningEvent, new ContextMenuEventHandler(AssocisatedObject_ContextMenuOpening), true);
        }

        /// <summary>
        /// block Context menu opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssocisatedObject_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// Open context menu when button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            Button source = sender as Button;
            if (source != null && source.ContextMenu != null)
            {
                source.Background = Application.Current.Resources["ButtonHoverBrush"] as SolidColorBrush;

                if (!isContextMenuOpen)
                {
                    // Add handler to detect when the ContextMenu closes
                    source.ContextMenu.AddHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed), true);
                    // If there is a drop-down assigned to this button, then position and display it
                    source.ContextMenu.PlacementTarget = source;
                    source.ContextMenu.Placement = PlacementMode.Bottom;
                    source.ContextMenu.IsOpen = true;
                    isContextMenuOpen = true;
                }
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click));
        }

        /// <summary>
        /// Handles context menu closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            AssociatedObject.ClearValue(Control.BackgroundProperty);

            isContextMenuOpen = false;
            var contextMenu = sender as ContextMenu;
            if (contextMenu != null)
            {
                contextMenu.RemoveHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed));
            }
        }
    }
}
