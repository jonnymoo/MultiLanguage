using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using System.Web.UI;
using System.Web;

namespace Civica.C360.Language
{

    [ExpressionPrefix("Lang")]
    public class LangExpressionBuilder : ExpressionBuilder
    {
        public override CodeExpression GetCodeExpression(BoundPropertyEntry entry,  object parsedData, ExpressionBuilderContext context)
        {
            CodeTypeReferenceExpression thisType = new CodeTypeReferenceExpression(base.GetType());
            CodePrimitiveExpression expression = new CodePrimitiveExpression(entry.Expression.Trim().ToString());

            return new CodeMethodInvokeExpression(thisType, "Translate", new CodeExpression[] { expression });
        }

        public static string Translate(string expression)
        {
            return new Translator(new Language(HttpContext.Current), new FileLanguagePackService()).Translate("", expression);
        }

        public override bool SupportsEvaluate
        {
            get
            {
                return true;
            }
        }
    }
}
