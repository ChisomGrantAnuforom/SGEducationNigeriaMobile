using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGEducationNigeriaMobile.Models;

namespace SGEducationNigeriaMobile.Pages.User;

public partial class ArticleDetailPage : ContentPage
{
    public ArticleDetailPage(Article article) 
    {
        InitializeComponent();
        BindingContext = article;
    }
}