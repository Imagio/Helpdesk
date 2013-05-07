using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Imagio.Helpdesk.TemplateSelector
{
    public class GenericTemplateSelector : DataTemplateSelector
    {
        static GenericTemplateSelector()
        {

        }

        public static readonly DependencyProperty TemplatesProperty =
            DependencyProperty.RegisterAttached("Templates", typeof(TemplateCollection), typeof(DataTemplateSelector),
            new FrameworkPropertyMetadata(new TemplateCollection(), FrameworkPropertyMetadataOptions.Inherits));

        public static TemplateCollection GetTemplates(UIElement element)
        {
            return (TemplateCollection)element.GetValue(TemplatesProperty);
        }

        public static void SetTemplates(UIElement element, TemplateCollection collection)
        {
            element.SetValue(TemplatesProperty, collection);
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is UIElement))
                return base.SelectTemplate(item, container);

            TemplateCollection templates = GetTemplates(container as UIElement);
            if (templates == null || templates.Count == 0)
                base.SelectTemplate(item, container);

            if (item != null && item.GetType().IsGenericType)
            {
                Type[] typeParams = item.GetType().GetGenericArguments();
                Type workspaceParam = typeParams[0];
                if (templates != null)
                    foreach (var template in templates)
                        if (template.Value == workspaceParam)
                            return template.DataTemplate;
            }


            return base.SelectTemplate(item, container);
        }
    }

    public class TemplateCollection : List<Template>
    {
    }

    public class Template : DependencyObject
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Type), typeof(Template));

        public static readonly DependencyProperty DataTemplateProperty =
           DependencyProperty.Register("DataTemplate", typeof(DataTemplate), typeof(Template));

        public Type Value
        { get { return (Type)GetValue(ValueProperty); } set { SetValue(ValueProperty, value); } }

        public DataTemplate DataTemplate
        { get { return (DataTemplate)GetValue(DataTemplateProperty); } set { SetValue(DataTemplateProperty, value); } }
    }
}
