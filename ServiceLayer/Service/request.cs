using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Model.ServiceModel;
using System.Diagnostics;

namespace LiroInspectServiceModel.Services
{
	class request<T, K> where K : IResult, new()
	{
		internal static K executePost(T reqObj, Uri uri)
		{
			using (var client = new HttpClient())
			{
				try
				{
					// tempory set this value to 5 minutes, was 900
					client.Timeout = TimeSpan.FromSeconds(300);
					var data = new StringContent(JsonConvert.SerializeObject(reqObj),
						Encoding.UTF8, "application/json");
					var response = client.PostAsync(uri, data).Result;
					if (response.IsSuccessStatusCode)
					{
						
						var res = (K)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, typeof(K));
						return res;
					}
					else
					{
						
						K k = (K)JsonConvert.DeserializeObject(
							response.Content.ReadAsStringAsync()
							.Result, typeof(K));
						if (k.result == null)
						{
							k.result = new Result()
							{
								code = -1,//response.StatusCode,
								message = "",
								type = "Error",
							};

						}
						return k;
					}
				}
				catch (Exception ex)
				{
					K k = new K();

					if (ex != null && ex.InnerException.ToString() == "A task was canceled")
					{
						k.result = new Result()
						{
							code = -2,
							message = "Timed out",
							type = "Exception",
						};
						Debug.WriteLine("http timeout occured");
					}
					else
					{
						k.result = new Result()
						{
							code = -3,
							message = "HTTP Exception occured",
							type = "Exception",
						};

					}
					return k;
				}
			}

		}

		internal static K executeGet(T reqObj, Uri uri)
		{
			using (var client = new HttpClient())
			{
				client.Timeout = TimeSpan.FromSeconds(900);
				try
				{
					var response = client.GetAsync(uri).Result;
					if (response.IsSuccessStatusCode)
					{
						var res = (K)JsonConvert.DeserializeObject(
							response.Content.ReadAsStringAsync()
							.Result, typeof(K));
						return res;
					}
					else
					{
						K k = (K)JsonConvert.DeserializeObject(
							response.Content.ReadAsStringAsync()
							.Result, typeof(K));
						if (k.result == null)
						{
							k.result = new Result()
							{
								code = -1,//response.StatusCode,
								message = "",
								type = "Error",
							};
						}
						return k;
					}
				}
				catch (Exception ex)
				{
					K k = new K();
					if (ex != null && ex.InnerException.ToString() == "A task was canceled")
					{
						k.result = new Result()
						{
							code = -2,
							message = "Timed out",
							type = "Exception",
						};
					}
					else
					{
						k.result = new Result()
						{
							code = -3,
							message = "HTTP Exception occured",
							type = "Exception",
						};
					}
					return k;
				}
			}
		}
	}
}
