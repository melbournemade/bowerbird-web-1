﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Bowerbird.Core.Utilities;
using Bowerbird.Web.ViewModels;
using Bowerbird.Core.Config;

namespace Bowerbird.Web.Validators
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidFileAttribute : ValidationAttribute
    {

        /// <summary>
        /// A valid file is one which is not null and is of a type (ie its mimetype) that is supported
        /// </summary>
        public override bool IsValid(Object value)
        {
            var input = value as MediaResourceCreateInput;

            if (input == null || input.Type.ToLower() != "file") return true;

            return 
                !string.IsNullOrWhiteSpace(input.FileName) &&
                input.File != null && 
                IsSupportedFile(input.File.InputStream, input.File.FileName, input.File.ContentType);
        }

        /// <summary>
        /// Attempt to load the stream as an image and/or audio file. If either succeed, then
        /// check that the subsequent derived mimetype is on the supported list
        /// </summary>
        private static bool IsSupportedFile(Stream stream, string filename, string mimeType)
        {
            string foundMimeType = string.Empty;
            ImageUtility image;
            AudioUtility audio;

            if (ImageUtility.TryLoad(stream, out image))
            {
                foundMimeType = image.GetMimeType();
            }
            else if (AudioUtility.TryLoad(stream, filename, mimeType, out audio))
            {
                foundMimeType = audio.GetMimeType();
            }

            return MediaTypeUtility.IsSupportedMimeType(foundMimeType);
        }

    }
}