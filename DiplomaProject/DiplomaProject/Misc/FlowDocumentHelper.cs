using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DiplomaProject
{
    public class FlowDocumentHelper
    {
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.RegisterAttached(
            "Document", typeof (FlowDocument), typeof (FlowDocumentHelper), new PropertyMetadata(default(FlowDocument), DocumentPropertyChangedCallback));

        private static void DocumentPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var richTextBox = dependencyObject as RichTextBox;
            if(richTextBox == null) {
                return;
            }
            richTextBox.Document = dependencyPropertyChangedEventArgs.NewValue as FlowDocument;
        }

        public static void SetDocument(DependencyObject element, FlowDocument value)
        {
           element.SetValue(DocumentProperty, value);
        }

        public static FlowDocument GetDocument(DependencyObject element)
        {
            return (FlowDocument) element.GetValue(DocumentProperty);
        }

        public static readonly DependencyProperty StyleNameProperty = DependencyProperty.RegisterAttached("StyleName",
            typeof (String), typeof (FlowDocumentHelper), new PropertyMetadata(default(String)));

        public static void SetStyleName(DependencyObject element, String value)
        {
            element.SetValue(StyleNameProperty, value);
        }

        public static String GetStyleName(DependencyObject element)
        {
            return (String) element.GetValue(StyleNameProperty);
        }
    }
}