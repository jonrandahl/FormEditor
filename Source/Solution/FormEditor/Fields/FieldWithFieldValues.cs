﻿using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace FormEditor.Fields
{
	public abstract class FieldWithFieldValues : FieldWithMandatoryValidation
	{
		public FieldValue[] FieldValues { get; set; }

		protected internal override bool ValidateSubmittedValue(IEnumerable<Field> allCollectedValues, IPublishedContent content)
		{
			if(base.ValidateSubmittedValue(allCollectedValues, content) == false)
			{
				return false;
			}
			if(string.IsNullOrEmpty(SubmittedValue))
			{
				// nothing selected => valid (mandatory validation is handled by base class)
				return true;
			}

			var submittedFieldValues = SubmittedValue.Split(',');
			FieldValues.ToList().ForEach(f => f.Selected = submittedFieldValues.Contains(f.Value));

			// make sure all submitted values are actually defined as a field value (maybe some schmuck tampered with the options client side)
			if (submittedFieldValues.Any())
			{
				return submittedFieldValues.All(v => FieldValues.Any(f => f.Value == v));
			}

			return true;
		}

		public virtual bool IsMultiSelectEnabled
		{
			get { return false; }
		}
	}
}