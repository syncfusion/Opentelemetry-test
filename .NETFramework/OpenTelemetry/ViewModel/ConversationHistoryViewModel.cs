

using Syncfusion.UI.Xaml.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace OpenTelemetry.ViewModel
{
    internal class ConversationHistoryViewModel : INotifyPropertyChanged
    {

        private Author currentUser;
        public Author CurrentUser
        {
            get
            {
                return this.currentUser;
            }
            set
            {
                this.currentUser = value;
            }
        }

        public SfAIAssistView assistView
        {
            get;
            set;
        }

        private ObservableCollection<object> chats;
        public ObservableCollection<object> Chats
        {
            get
            {
                return this.chats;
            }
            set
            {
                this.chats = value;
            }
        }
        private ObservableCollection<AssistConversationItem> conversations;
        public ObservableCollection<AssistConversationItem> Conversations
        {
            get { return this.conversations; }
            set
            {
                this.conversations = value;
            }
        }

        //private ObservableCollection<AssistConversationItem> agents;

        //public ObservableCollection<AssistAgentItem> Agents
        //{
        //    get { return this.agents; }
        //    set { this.agents = value; }
        //}

        //private AssistAgentItem agent;

        //public AssistAgentItem Agent
        //{
        //    get { return this.agent; }
        //    set { this.agent = value; }
        //}

        private bool showTypingIndicator;
        public bool ShowTypingIndicator
        {
            get
            {
                return this.showTypingIndicator;
            }
            set
            {
                this.showTypingIndicator = value;
                RaisePropertyChanged("ShowTypingIndicator");
            }
        }

        private IEnumerable<string> suggestion;
        public IEnumerable<string> Suggestion
        {
            get
            {
                return this.suggestion;
            }
            set
            {
                this.suggestion = value;
                RaisePropertyChanged("Suggestion");
            }
        }

        internal bool isStopResponding;

        private readonly AzureOpenAIClient openAIClient;
        private readonly ChatClient chatClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public DataTemplate AIIcon { get; set; }

        public async Task SendUserMessageAsync(string prompt)
        {
            ShowTypingIndicator = true;

            try
            {
                ChatCompletion completion = await chatClient.CompleteChatAsync(
                    new List<ChatMessage>
                    {
                new UserChatMessage(prompt)
                    });

                string response =
                    completion.Content.FirstOrDefault()?.Text ??
                    "No response received.";

                Chats.Add(new TextMessage
                {
                    Author = new Author { Name = "Bot", ContentTemplate = AIIcon },
                    DateTime = DateTime.Now,
                    Text = response
                });
            }
            catch (Exception ex)
            {
                Chats.Add(new TextMessage
                {
                    Author = new Author { Name = "Bot", ContentTemplate = AIIcon },
                    DateTime = DateTime.Now,
                    Text = $"Error: {ex.Message}"
                });
            }
            finally
            {
                ShowTypingIndicator = false;
                isStopResponding = false;
            }
        }
        public ConversationHistoryViewModel()
        {

            //SelectedModel = Models[1];

            string endpoint = "https://openaijegan.openai.azure.com/";
            string apiKey = "c71665b4639d46a5917c4a15f33e57a4";
            string deploymentName = "gpt-4o-mini";

            openAIClient = new AzureOpenAIClient(
                new Uri(endpoint),
                new AzureKeyCredential(apiKey));

            chatClient = openAIClient.GetChatClient(deploymentName);
            // this.InitializePromptLibrary();
            this.Suggestion = new ObservableCollection<string>();
            this.Suggestion = new ObservableCollection<string>(new string[] { "Tamil", "English", "Maths" });


            this.Chats = new ObservableCollection<object>();
            this.CurrentUser = new Author { Name = "John" };
            var AI = new Author { Name = "Bot" };

            Chats.Add(new TextMessage
            {
                Text = "TAMILSELVAN",
                DateTime = DateTime.Now,
                Author = CurrentUser,
            });

            this.Conversations = new ObservableCollection<AssistConversationItem>()
            {
                new AssistConversationItem
                {
                    Title = "Best Practices for API Design",
                    DateTime = DateTime.Now.AddDays(-1),
                    AssistItems = new ObservableCollection<object>
                    {
                        new TextMessage
                        {
                            Author = CurrentUser,
                            DateTime = DateTime.Now.AddDays(-1),
                            Text = "Best Practices for API Design"
                        },
                        new TextMessage
                        {
                            Author = AI,
                            DateTime = DateTime.Now.AddDays(-1).AddMinutes(1),
                            Text = "**Best Practices for API Design:**\n\n" +
                                   "1. Use RESTful principles with clear resource models and proper HTTP methods (GET for retrieval, POST for creation, PUT for updates, DELETE for removal).\n" +
                                   "2. Implement API versioning (URL-based or header-based) to maintain backward compatibility while introducing new features.\n" +
                                   "3. Provide comprehensive API documentation using tools like Swagger/OpenAPI with examples, error codes, and authentication details.\n" +
                                   "4. Design consistent error responses, implement rate limiting, and use proper status codes to communicate API behavior clearly."
                        }
                    }
                },
                new AssistConversationItem
                {
                    Title = "Cloud Migration Strategy",
                    DateTime = DateTime.Now.AddDays(-2),
                    AssistItems = new ObservableCollection<object>
                    {
                        new TextMessage
                        {
                            Author = CurrentUser,
                            DateTime = DateTime.Now.AddDays(-2),
                            Text = "Cloud Migration Strategy"
                        },
                        new TextMessage
                        {
                            Author = AI,
                            DateTime = DateTime.Now.AddDays(-2).AddMinutes(1),
                            Text = "**Cloud Migration Strategy:**\n\n" +
                                   "1. Assess current infrastructure by documenting applications, dependencies, performance metrics, and security requirements in detail.\n" +
                                   "2. Choose appropriate cloud services (IaaS, PaaS, SaaS) based on workload requirements, cost analysis, and organizational expertise.\n" +
                                   "3. Create a phased migration plan starting with non-critical systems, conducting thorough testing at each stage, and maintaining rollback procedures.\n" +
                                   "4. Monitor post-migration performance, optimize resource allocation, and continuously review costs to ensure ROI."
                        }
                    }
                },
                new AssistConversationItem
                {
                    Title = "Database Optimization Tips",
                    DateTime = DateTime.Now.AddDays(-3),
                    AssistItems = new ObservableCollection<object>
                    {
                        new TextMessage
                        {
                            Author = CurrentUser,
                            DateTime = DateTime.Now.AddDays(-3),
                            Text = "Database Optimization Tips"
                        },
                        new TextMessage
                        {
                            Author = AI,
                            DateTime = DateTime.Now.AddDays(-3).AddMinutes(1),
                            Text = "**Database Optimization Tips:**\n\n" +
                                   "1. Implement proper indexing on frequently queried columns, but avoid over-indexing which can slow down write operations.\n" +
                                   "2. Optimize queries by using EXPLAIN plans, eliminating N+1 queries, and using appropriate JOIN strategies for your database system.\n" +
                                   "3. Implement connection pooling to reuse database connections efficiently and reduce overhead from establishing new connections.\n" +
                                   "4. Use caching strategies (Redis, Memcached) for frequently accessed data, and consider database partitioning for very large datasets to improve query performance."
                        }
                    }
                },
                new AssistConversationItem
                {
                    Title = "Security Considerations",
                    DateTime = DateTime.Now.AddDays(-4),
                    AssistItems = new ObservableCollection<object>
                    {
                        new TextMessage
                        {
                            Author = CurrentUser,
                            DateTime = DateTime.Now.AddDays(-4),
                            Text = "Security Considerations"
                        },
                        new TextMessage
                        {
                            Author = AI,
                            DateTime = DateTime.Now.AddDays(-4).AddMinutes(1),
                            Text = "**Security Considerations:**\n\n" +
                                   "1. Implement robust authentication mechanisms (OAuth 2.0, JWT) and authorization frameworks (RBAC, ABAC) to control access to sensitive resources.\n" +
                                   "2. Encrypt sensitive data h in transit (TLS/SSL) and at rest using industry-standard encryption algorithms like AES-256.\n" +
                                   "3. Validate all user inputs on the server-side to prevent injection attacks, XSS, and other common vulnerabilities.\n" +
                                   "4. Conduct regular security audits, penetration testing, and stay updated with security patches to protect against emerging threats."
                        }
                    }
                },
                new AssistConversationItem
                {
                    Title = "Performance Tuning",
                    DateTime = DateTime.Now.AddDays(-5),
                    AssistItems = new ObservableCollection<object>
                    {
                        new TextMessage
                        {
                            Author = CurrentUser,
                            DateTime = DateTime.Now.AddDays(-5),
                            Text = "Performance Tuning"
                        },
                        new TextMessage
                        {
                            Author = AI,
                            DateTime = DateTime.Now.AddDays(-5).AddMinutes(1),
                            Text = "**Performance Tuning:**\n\n" +
                                   "1. Profile your code using tools like dotTrace or profilers to identify tlenecks and resource-intensive operations accurately.\n" +
                                   "2. Optimize algorithms by choosing appropriate data structures, reducing time complexity, and avoiding unnecessary computations.\n" +
                                   "3. Implement caching layers (distributed caches) and asynchronous processing to handle high loads and improve response times.\n" +
                                   "4. Use load balancing techniques and horizontal scaling to distribute traffic across multiple servers and ensure system reliability."
                        }
                    }
                }
            };

            // Initialize new collections for agent navigation support
//            this.Agents = new ObservableCollection<AssistAgentItem>()
//{
//    new AssistAgentItem
//    {
//        Name = "Writing Assistant",
//        Description = "Helps with writing, editing, and brainstorming.",
//        Instructions = "Assist users with content creation, proofreading, and idea generation.",
//        IconData =
//            "M1.5 0C0.671573 0 0 0.671573 0 1.5V12.5C0 13.3284 0.671572 14 1.5 14H6.37731C6.13619 13.6992 5.93763 13.3628 5.79076 13H1.5C1.22386 13 1 12.7761 1 12.5V1.5C1 1.22386 1.22386 1 1.5 1H10.5C10.7761 1 11 1.22386 11 1.5V7.79076C11.3628 7.93763 11.6992 8.13619 12 8.37731V1.5C12 0.671573 11.3284 0 10.5 0H1.5Z" +
//            "M6.37731 9H2.5C2.22386 9 2 9.22386 2 9.5C2 9.77614 2.22386 10 2.5 10H5.79076C5.93763 9.63717 6.13619 9.30079 6.37731 9Z" +
//            "M2.5 3C2.22386 3 2 3.22386 2 3.5C2 3.77614 2.22386 4 2.5 4H9.5C9.77614 4 10 3.77614 10 3.5C10 3.22386 9.77614 3 9.5 3H2.5Z" +
//            "M2.5 6C2.22386 6 2 6.22386 2 6.5C2 6.77614 2.22386 7 2.5 7H9.5C9.77614 7 10 6.77614 10 6.5C10 6.22386 9.77614 6 9.5 6H2.5Z" +
//            "M13 11.5C13 11.8236 12.9578 12.1346 12.8735 12.4331C12.7915 12.7293 12.6742 13.0073 12.5215 13.2671C12.3711 13.5269 12.1888 13.7638 11.9746 13.978C11.7627 14.1899 11.5269 14.3722 11.2671 14.5249C11.0073 14.6753 10.7282 14.7926 10.4297 14.877C10.1312 14.959 9.82129 15 9.5 15C9.17643 15 8.8654 14.959 8.56689 14.877C8.27067 14.7926 7.99268 14.6753 7.73291 14.5249C7.47314 14.3722 7.23617 14.1899 7.02197 13.978C6.81006 13.7638 6.62777 13.5269 6.4751 13.2671C6.32471 13.0073 6.20736 12.7282 6.12305 12.4297C6.04102 12.1312 6 11.8213 6 11.5C6 11.1787 6.04102 10.8688 6.12305 10.5703C6.20736 10.2718 6.32471 9.99268 6.4751 9.73291C6.62777 9.47314 6.81006 9.2373 7.02197 9.02539C7.23617 8.8112 7.47314 8.62891 7.73291 8.47852C7.99268 8.32585 8.27181 8.2085 8.57031 8.12646C8.86882 8.04216 9.17871 8 9.5 8C9.82129 8 10.1312 8.04216 10.4297 8.12646C10.7282 8.2085 11.0073 8.32585 11.2671 8.47852C11.5269 8.62891 11.7627 8.8112 11.9746 9.02539C12.1888 9.2373 12.3711 9.47314 12.5215 9.73291C12.6742 9.99268 12.7915 10.2718 12.8735 10.5703C12.9578 10.8688 13 11.1787 13 11.5ZM8.3208 10.1875H10.6792C10.661 10.0986 10.6348 9.99382 10.6006 9.87305C10.5664 9.75228 10.5243 9.62695 10.4741 9.49707C10.424 9.36719 10.3659 9.23958 10.2998 9.11426C10.236 8.98665 10.1631 8.87272 10.0811 8.77246C10.0013 8.6722 9.91243 8.59131 9.81445 8.52979C9.71875 8.46826 9.61393 8.4375 9.5 8.4375C9.38607 8.4375 9.28011 8.46826 9.18213 8.52979C9.08643 8.59131 8.99756 8.6722 8.91553 8.77246C8.83577 8.87272 8.76286 8.98665 8.69678 9.11426C8.63298 9.23958 8.57601 9.36719 8.52588 9.49707C8.47575 9.62695 8.43359 9.75228 8.39941 9.87305C8.36523 9.99382 8.33903 10.0986 8.3208 10.1875ZM6.73145 10.1875H7.87305C7.92546 9.90495 8.00065 9.62923 8.09863 9.36035C8.19889 9.09147 8.33219 8.83968 8.49854 8.60498C8.30713 8.66423 8.1237 8.74626 7.94824 8.85107C7.77279 8.95589 7.60872 9.0778 7.45605 9.2168C7.30566 9.35579 7.16781 9.50732 7.04248 9.67139C6.91943 9.83545 6.81576 10.0075 6.73145 10.1875ZM10.5015 8.60498C10.6678 8.83968 10.8 9.09147 10.8979 9.36035C10.9982 9.62923 11.0745 9.90495 11.127 10.1875H12.2686C12.182 10.0075 12.0771 9.83545 11.9541 9.67139C11.8311 9.50732 11.6932 9.35579 11.5405 9.2168C11.3901 9.0778 11.2272 8.95589 11.0518 8.85107C10.8763 8.74626 10.6929 8.66423 10.5015 8.60498ZM7.80469 12.375C7.78418 12.2292 7.76937 12.0845 7.76025 11.9409C7.75342 11.7951 7.75 11.6481 7.75 11.5C7.75 11.3519 7.75342 11.2061 7.76025 11.0625C7.76937 10.9167 7.78418 10.7708 7.80469 10.625H6.56396C6.52295 10.7686 6.49105 10.9132 6.46826 11.0591C6.44775 11.2026 6.4375 11.3496 6.4375 11.5C6.4375 11.6504 6.44775 11.7985 6.46826 11.9443C6.49105 12.0879 6.52295 12.2314 6.56396 12.375H7.80469ZM10.7578 12.375C10.7783 12.2292 10.792 12.0845 10.7988 11.9409C10.8079 11.7951 10.8125 11.6481 10.8125 11.5C10.8125 11.3519 10.8079 11.2061 10.7988 11.0625C10.792 10.9167 10.7783 10.7708 10.7578 10.625H8.24219C8.22168 10.7708 8.20687 10.9167 8.19775 11.0625C8.19092 11.2061 8.1875 11.3519 8.1875 11.5C8.1875 11.6481 8.19092 11.7951 8.19775 11.9409C8.20687 12.0845 8.22168 12.2292 8.24219 12.375H10.7578ZM12.436 10.625H11.1953C11.2158 10.7708 11.2295 10.9167 11.2363 11.0625C11.2454 11.2061 11.25 11.3519 11.25 11.5C11.25 11.6481 11.2454 11.7951 11.2363 11.9409C11.2295 12.0845 11.2158 12.2292 11.1953 12.375H12.436C12.4771 12.2314 12.5078 12.0879 12.5283 11.9443C12.5511 11.7985 12.5625 11.6504 12.5625 11.5C12.5625 11.3496 12.5511 11.2026 12.5283 11.0591C12.5078 10.9132 12.4771 10.7686 12.436 10.625ZM8.49854 14.395C8.33219 14.1603 8.19889 13.9085 8.09863 13.6396C8.00065 13.3708 7.92546 13.0951 7.87305 12.8125H6.73145C6.81576 12.9902 6.91943 13.1623 7.04248 13.3286C7.16553 13.4927 7.30339 13.6442 7.45605 13.7832C7.60872 13.9199 7.77279 14.0418 7.94824 14.1489C8.1237 14.2537 8.30713 14.3358 8.49854 14.395ZM10.6792 12.8125H8.3208C8.33903 12.9014 8.36523 13.0062 8.39941 13.127C8.43359 13.2477 8.47575 13.373 8.52588 13.5029C8.57601 13.6328 8.63298 13.7616 8.69678 13.8892C8.76286 14.0145 8.83577 14.1273 8.91553 14.2275C8.99756 14.3278 9.08643 14.4087 9.18213 14.4702C9.28011 14.5317 9.38607 14.5625 9.5 14.5625C9.61393 14.5625 9.71875 14.5317 9.81445 14.4702C9.91243 14.4087 10.0013 14.3278 10.0811 14.2275C10.1631 14.1273 10.236 14.0145 10.2998 13.8892C10.3659 13.7616 10.424 13.6328 10.4741 13.5029C10.5243 13.373 10.5664 13.2477 10.6006 13.127C10.6348 13.0062 10.661 12.9014 10.6792 12.8125ZM12.2686 12.8125H11.127C11.0745 13.0951 10.9982 13.3708 10.8979 13.6396C10.8 13.9085 10.6678 14.1603 10.5015 14.395C10.6929 14.3358 10.8763 14.2537 11.0518 14.1489C11.2272 14.0441 11.3901 13.9222 11.5405 13.7832C11.6932 13.6442 11.8311 13.4927 11.9541 13.3286C12.0771 13.1646 12.182 12.9925 12.2686 12.8125Z"
//    },
//    new AssistAgentItem
//    {
//        Name = "Art Assistant",
//        Description = "Helps create artwork, illustrations, and design concepts.",
//        Instructions = "Generate creative ideas and art-related assistance.",
//        IconData =
//            "M11.5 2.5C11.5 3.05228 11.0523 3.5 10.5 3.5C9.94771 3.5 9.5 3.05228 9.5 2.5C9.5 1.94772 9.94771 1.5 10.5 1.5C11.0523 1.5 11.5 1.94772 11.5 2.5Z" +
//            "M1.5 0C0.671573 0 0 0.671573 0 1.5V10.5C0 11.3284 0.671573 12 1.5 12H7.7289C7.6282 11.6794 7.55893 11.3448 7.52469 11H1.5C1.22386 11 1 10.7761 1 10.5V8.61803L3.17658 4.26486C3.34827 3.92148 3.82687 3.89169 4.03982 4.21112L6.12813 7.34358C6.68207 8.1745 7.83515 8.21774 8.46767 7.54303C8.52183 7.4693 8.57796 7.39712 8.63598 7.32655C8.66533 7.28073 8.69269 7.23266 8.71784 7.18235L9.32674 5.96456C9.47936 5.65931 9.88618 5.59329 10.1275 5.83461L10.3011 6.00821C10.6294 5.84719 10.978 5.72111 11.3419 5.6348L10.8346 5.1275C10.1107 4.40354 8.89018 4.6016 8.43231 5.51734L7.82342 6.73513C7.65173 7.07852 7.17313 7.10831 6.96018 6.78888L4.87187 3.65642C4.23301 2.69812 2.79723 2.78751 2.28216 3.81765L1 6.38197V1.5C1 1.22386 1.22386 1 1.5 1H12.5C12.7761 1 13 1.22386 13 1.5V5.52469C13.3448 5.55893 13.6794 5.6282 14 5.7289V1.5C14 0.671573 13.3284 0 12.5 0H1.5Z" +
//            "M16 10C16 10.3698 15.9518 10.7253 15.8555 11.0664C15.7617 11.4049 15.6276 11.7227 15.4531 12.0195C15.2812 12.3164 15.0729 12.5872 14.8281 12.832C14.5859 13.0742 14.3164 13.2826 14.0195 13.457C13.7227 13.6289 13.4036 13.763 13.0625 13.8594C12.7214 13.9531 12.3672 14 12 14C11.6302 14 11.2747 13.9531 10.9336 13.8594C10.5951 13.763 10.2773 13.6289 9.98047 13.457C9.68359 13.2826 9.41276 13.0742 9.16797 12.832C8.92578 12.5872 8.71745 12.3164 8.54297 12.0195C8.37109 11.7227 8.23698 11.4036 8.14062 11.0625C8.04688 10.7214 8 10.3672 8 10C8 9.63281 8.04688 9.27865 8.14062 8.9375C8.23698 8.59635 8.37109 8.27734 8.54297 7.98047C8.71745 7.68359 8.92578 7.41406 9.16797 7.17188C9.41276 6.92708 9.68359 6.71875 9.98047 6.54688C10.2773 6.3724 10.5964 6.23828 10.9375 6.14453C11.2786 6.04818 11.6328 6 12 6C12.3672 6 12.7214 6.04818 13.0625 6.14453C13.4036 6.23828 13.7227 6.3724 14.0195 6.54688C14.3164 6.71875 14.5859 6.92708 14.8281 7.17188C15.0729 7.41406 15.2812 7.68359 15.4531 7.98047C15.6276 8.27734 15.7617 8.59635 15.8555 8.9375C15.9518 9.27865 16 9.63281 16 10ZM10.6523 8.5H13.3477C13.3268 8.39844 13.2969 8.27865 13.2578 8.14062C13.2188 8.0026 13.1706 7.85938 13.1133 7.71094C13.056 7.5625 12.9896 7.41667 12.9141 7.27344C12.8411 7.1276 12.7578 6.9974 12.6641 6.88281C12.5729 6.76823 12.4714 6.67578 12.3594 6.60547C12.25 6.53516 12.1302 6.5 12 6.5C11.8698 6.5 11.7487 6.53516 11.6367 6.60547C11.5273 6.67578 11.4258 6.76823 11.332 6.88281C11.2409 6.9974 11.1576 7.1276 11.082 7.27344C11.0091 7.41667 10.944 7.5625 10.8867 7.71094C10.8294 7.85938 10.7812 8.0026 10.7422 8.14062C10.7031 8.27865 10.6732 8.39844 10.6523 8.5ZM8.83594 8.5H10.1406C10.2005 8.17708 10.2865 7.86198 10.3984 7.55469C10.513 7.2474 10.6654 6.95964 10.8555 6.69141C10.6367 6.75911 10.4271 6.85286 10.2266 6.97266C10.026 7.09245 9.83854 7.23177 9.66406 7.39062C9.49219 7.54948 9.33464 7.72266 9.19141 7.91016C9.05078 8.09766 8.93229 8.29427 8.83594 8.5ZM13.1445 6.69141C13.3346 6.95964 13.4857 7.2474 13.5977 7.55469C13.7122 7.86198 13.7995 8.17708 13.8594 8.5H15.1641C15.0651 8.29427 14.9453 8.09766 14.8047 7.91016C14.6641 7.72266 14.5065 7.54948 14.332 7.39062C14.1602 7.23177 13.974 7.09245 13.7734 6.97266C13.5729 6.85286 13.3633 6.75911 13.1445 6.69141ZM10.0625 11C10.0391 10.8333 10.0221 10.668 10.0117 10.5039C10.0039 10.3372 10 10.1693 10 10C10 9.83073 10.0039 9.66406 10.0117 9.5C10.0221 9.33333 10.0391 9.16667 10.0625 9H8.64453C8.59766 9.16406 8.5612 9.32943 8.53516 9.49609C8.51172 9.66016 8.5 9.82812 8.5 10C8.5 10.1719 8.51172 10.3411 8.53516 10.5078C8.5612 10.6719 8.59766 10.8359 8.64453 11H10.0625ZM13.4375 11C13.4609 10.8333 13.4766 10.668 13.4844 10.5039C13.4948 10.3372 13.5 10.1693 13.5 10C13.5 9.83073 13.4948 9.66406 13.4844 9.5C13.4766 9.33333 13.4609 9.16667 13.4375 9H10.5625C10.5391 9.16667 10.5221 9.33333 10.5117 9.5C10.5039 9.66406 10.5 9.83073 10.5 10C10.5 10.1693 10.5039 10.3372 10.5117 10.5039C10.5221 10.668 10.5391 10.8333 10.5625 11H13.4375ZM15.3555 9H13.9375C13.9609 9.16667 13.9766 9.33333 13.9844 9.5C13.9948 9.66406 14 9.83073 14 10C14 10.1693 13.9948 10.3372 13.9844 10.5039C13.9766 10.668 13.9609 10.8333 13.9375 11H15.3555C15.4023 10.8359 15.4375 10.6719 15.4609 10.5078C15.487 10.3411 15.5 10.1719 15.5 10C15.5 9.82812 15.487 9.66016 15.4609 9.49609C15.4375 9.32943 15.4023 9.16406 15.3555 9ZM10.8555 13.3086C10.6654 13.0404 10.513 12.7526 10.3984 12.4453C10.2865 12.138 10.2005 11.8229 10.1406 11.5H8.83594C8.93229 11.7031 9.05078 11.8997 9.19141 12.0898C9.33203 12.2773 9.48958 12.4505 9.66406 12.6094C9.83854 12.7656 10.026 12.9049 10.2266 13.0273C10.4271 13.1471 10.6367 13.2409 10.8555 13.3086ZM13.3477 11.5H10.6523C10.6732 11.6016 10.7031 11.7214 10.7422 11.8594C10.7812 11.9974 10.8294 12.1406 10.8867 12.2891C10.944 12.4375 11.0091 12.5846 11.082 12.7305C11.1576 12.8737 11.2409 13.0026 11.332 13.1172C11.4258 13.2318 11.5273 13.3242 11.6367 13.3945C11.7487 13.4648 11.8698 13.5 12 13.5C12.1302 13.5 12.25 13.4648 12.3594 13.3945C12.4714 13.3242 12.5729 13.2318 12.6641 13.1172C12.7578 13.0026 12.8411 12.8737 12.9141 12.7305C12.9896 12.5846 13.056 12.4375 13.1133 12.2891C13.1706 12.1406 13.2188 11.9974 13.2578 11.8594C13.2969 11.7214 13.3268 11.6016 13.3477 11.5ZM15.1641 11.5H13.8594C13.7995 11.8229 13.7122 12.138 13.5977 12.4453C13.4857 12.7526 13.3346 13.0404 13.1445 13.3086C13.3633 13.2409 13.5729 13.1471 13.7734 13.0273C13.974 12.9076 14.1602 12.7682 14.332 12.6094C14.5065 12.4505 14.6641 12.2773 14.8047 12.0898C14.9453 11.9023 15.0651 11.7057 15.1641 11.5Z"
//    }
//};

//            if (this.Agents.Count > 0)
//            {
//                this.Agent = this.Agents[0];
//            }
        }
    }
}

