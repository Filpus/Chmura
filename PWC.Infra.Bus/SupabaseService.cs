using Supabase;
using Supabase.Interfaces;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PWC.Infra.Bus
{
    public class SupabaseService<T> where T : BaseModel, new()
    {
        private readonly Client _client;

        // Konstruktor inicjalizujący klienta Supabase
        public SupabaseService(string url = "https://mbzcixmvfpzqsmfowedr.supabase.co", string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1iemNpeG12ZnB6cXNtZm93ZWRyIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDM2MjQ5MDUsImV4cCI6MjA1OTIwMDkwNX0.obfFwltEEIOqQSgKOLGF2AhaT0-f_4bXtIdrkHpDT_k")
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true, // Opcjonalne: włączenie obsługi danych w czasie rzeczywistym
                AutoRefreshToken = true, // Opcjonalne: automatyczne odnawianie tokena
                
            };

            _client = new Client(url, key, options);
        }

        // Metoda inicjalizująca połączenie z Supabase
        public async Task InitializeAsync()
        {
            await _client.InitializeAsync();
        }

        // Pobieranie wszystkich rekordów z tabeli
        public async Task<List<T>> GetAllAsync()
        {
            var response = await _client.From<T>().Get();
            return response.Models;
        }

        // Pobieranie pojedynczego rekordu na podstawie ID (zakładamy, że ID to int lub string)
        public async Task<T?> GetByIdAsync(object id)
        {
            var response = await _client.From<T>()
                                        .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id) // Poprawne użycie filtra zamiast Where
                                        .Single();
            return response;
        }

        // Dodawanie nowego rekordu do tabeli
        public async Task AddAsync(T entity)
        {
            await _client.From<T>().Insert(entity);
        }


    }
}
