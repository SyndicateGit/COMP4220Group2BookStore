using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class BookQuotes
    {
        public DataSet getBookQuotes()
        {
            DALBookQuotes bookQuotes = new DALBookQuotes();
            return bookQuotes.getQuotes();
        }

        public bool addBookQuote(string quote_title, string quote_author, string quote)
        {
            DALBookQuotes addquote = new DALBookQuotes();
            return addquote.addBookQuote(quote_title, quote_author, quote);
        }
    }
}
