namespace Winner.Queue
{ 
   
    /// <summary>
    /// 分布式
    /// </summary>
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://Winner.Queue", ConfigurationName = "Winner.Queue.IQueueContract")]
    public interface IQueueContract
    {
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Queue.IQueueContract/Open", ReplyAction = "http://Winner.Queue.IQueueContract/OpenResponse")]
        void Open(string name, int maxCount);

        /// <summary>
        /// 存
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Queue.IQueueContract/Push", ReplyAction = "http://Winner.Queue.IQueueContract/PushResponse")]
        int Push(string name, object value);
        /// <summary>
        /// 取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Queue.IQueueContract/Pop", ReplyAction = "http://Winner.Queue.IQueueContract/PopResponse")]
        object Pop(string name);
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://Winner.Queue.IQueueContract/Close", ReplyAction = "http://Winner.Queue.IQueueContract/CloseResponse")]
        void Close(string name);
    }
}
