using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ClusterVR.CreatorKit.Translation;
using UnityEngine;

namespace Silksprite.PSCore.Drawers
{
    public class MergedIdStringValidator
    {
        const int MaxIdLength = 64;
        static readonly Regex ValidIdCharactersRegex = new(@"^[',\-.0-9A-Z_a-z]*$");

        readonly string _propertyDisplayName;

        public MergedIdStringValidator(string propertyDisplayName)
        {
            _propertyDisplayName = propertyDisplayName;
        }

        public bool IsValid(string mergedIdString, out IEnumerable<string> validationErrors)
        {
            var errors = new List<string>();
            
            if (!ValidIdCharactersRegex.IsMatch(mergedIdString))
            {
                var message = TranslationUtility.GetMessage(TranslationTable.cck_invalid_characters, _propertyDisplayName, _propertyDisplayName);
                errors.Add(message);
            }

            if (mergedIdString.Length > MaxIdLength)
            {
                var message = TranslationUtility.GetMessage(TranslationTable.cck_id_too_long, _propertyDisplayName, MaxIdLength);
                errors.Add(message);
            }

            validationErrors = errors;
            return errors.Count == 0;
        }
    }
}
