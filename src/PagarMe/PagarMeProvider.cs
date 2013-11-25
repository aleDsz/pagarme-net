﻿#region License

// The MIT License (MIT)
// 
// Copyright (c) 2013 Pagar.me
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using JetBrains.Annotations;
using PagarMe.Serializer;

namespace PagarMe
{
    /// <summary>
    /// Root class for accessing Pagar.me API
    /// </summary>
    public class PagarMeProvider
    {
        private readonly string _apiKey;
        private readonly PagarMeQueryable<Customer> _customers;
        private readonly string _encryptionKey;
        private readonly PagarMeQueryable<Transaction> _transactions;

        /// <summary>
        /// Instantiate a new PagarMeProvider
        /// </summary>
        /// <param name="apiKey">API key</param>
        /// <param name="encryptionKey">Encryption key</param>
        public PagarMeProvider(string apiKey, string encryptionKey)
        {
            _apiKey = apiKey;
            _encryptionKey = encryptionKey;
            _transactions = new PagarMeQueryable<Transaction>(this);
            _customers = new PagarMeQueryable<Customer>(this);
        }

        /// <summary>
        /// Currently used API key
        /// </summary>
        [PublicAPI]
        public string ApiKey
        {
            get { return _apiKey; }
        }

        /// <summary>
        /// Currently used encryption key
        /// </summary>
        [PublicAPI]
        public string EncryptionKey
        {
            get { return _encryptionKey; }
        }

        /// <summary>
        /// Transactions collection to be accessed via LINQ
        /// </summary>
        [PublicAPI]
        public PagarMeQueryable<Transaction> Transactions
        {
            get { return _transactions; }
        }

        /// <summary>
        /// Customers collection to be accessed via LINQ
        /// </summary>
        [PublicAPI]
        public PagarMeQueryable<Customer> Customers
        {
            get { return _customers; }
        }

        /// <summary>
        /// Creates a new transaction
        /// </summary>
        /// <param name="setup">Transaction data</param>
        /// <returns>Transaction object representing the new transaction</returns>
        [PublicAPI]
        public Transaction PostTransaction(TransactionSetup setup)
        {
            PagarMeQuery query = new PagarMeQuery(this, "POST", "transactions");

            foreach (var tuple in UrlSerializer.Serialize(setup))
                query.AddQuery(tuple.Item1, tuple.Item2);

            return new Transaction(this, query.Execute());
        }
    }
}