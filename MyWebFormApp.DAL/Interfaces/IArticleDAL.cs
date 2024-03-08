﻿using MyWebFormApp.BO;
using System.Collections.Generic;

namespace MyWebFormApp.DAL.Interfaces
{
    public interface IArticleDAL : ICrud<Article>
    {
        IEnumerable<Article> GetArticleWithCategory();
        IEnumerable<Article> GetArticleByCategory(int categoryId);
        IEnumerable<Article> GetWithPaging(int pageNumber, int pageSize);
        int GetCountArticles();
        int InsertWithIdentity(Article article);
        void InsertArticleWithCategory(Article article);
        IEnumerable<Article> GetWithPaging(int pageNumber, int pageSize, string name);
        int GetCountArticles(string name);
    }
}
