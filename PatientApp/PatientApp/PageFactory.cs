using PatientApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PatientApp
{
    public class CachedPages : Dictionary<Type, ContentPage>
    { }

    /// <summary>
    /// Cache manager for page instances
    /// </summary>
    public static class PagesFactory
    {
        //static readonly Dictionary<Type, BaseContentPage> pages = new Dictionary<Type, BaseContentPage>();

        //static readonly CachedPages pages_it = new CachedPages();
        //static readonly CachedPages pages_en = new CachedPages();

        static readonly Dictionary<string, CachedPages> languageCachedPages = new Dictionary<string, CachedPages>();

        /// <summary>
        /// Get an instance of a page of given type in a given language
        /// </summary>
        /// <typeparam name="T">The type of page</typeparam>
        /// <param name="lang">The language of cached page</param>
        /// <param name="cachePages">Request a cached instance page</param>
        /// <returns></returns>
        public static T GetPage<T>(string lang, bool cachePages = true) where T : ContentPage
        {
            Type pageType = typeof(T);

            if (cachePages)
            {
                var key = lang.ToLower();
                //var pages = lang.ToLower() == "it_it" ? pages_it : pages_en;
                CachedPages pages;
                if (!languageCachedPages.ContainsKey(key))
                {
                    pages = new CachedPages();
                    languageCachedPages.Add(key, pages);
                }
                else
                {
                    pages = languageCachedPages[key];
                }

                if (!pages.ContainsKey(pageType))
                {
                    BaseContentPage page = (BaseContentPage)Activator.CreateInstance(pageType);
                    pages.Add(pageType, page);
                }

                return pages[pageType] as T;
            }
            else
            {
                return Activator.CreateInstance(pageType) as T;
            }
        }

        /// <summary>
        /// Get an instance of a page of given type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachePages">Request a cached instance page</param>
        /// <returns></returns>
        public static T GetPage<T>(bool cachedPages = true) where T : ContentPage
        {
            return GetPage<T>("en_UK", cachedPages);
        }
      
    }
}