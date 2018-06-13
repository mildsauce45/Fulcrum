using Fulcrum.Models;

namespace Fulcrum
{
    /// <summary>
    /// This class is basically here to serve as a concrete implementation of a delegate for the emitter library that allows
    /// us to store and pass along context information to the HttpCallFactory class which does the actual work of making and HTTP request.
    /// </summary>
    internal class HttpCallProxy
    {
        private EndpointConfig _endpointConfig;
        private RequestSettings _settings;

        internal HttpCallProxy(RequestSettings settings, EndpointConfig config)
        {
            _settings = settings;
            _endpointConfig = config;
        }

        #region Proxy Methods

        public TRet NoParamCall<TRet>() =>
            MakeHttpCall<TRet>();

        public TRet SingleParamCall<TIn1, TRet>(TIn1 parm) =>
            MakeHttpCall<TRet>(parm);

        public TRet TwoParamCall<TIn1, TIn2, TRet>(TIn1 parm1, TIn2 parm2) =>
            MakeHttpCall<TRet>(parm1, parm2);

        public TRet ThreeParamCall<TIn1, TIn2, TIn3, TRet>(TIn1 parm1, TIn2 parm2, TIn3 parm3) =>
            MakeHttpCall<TRet>(parm1, parm2, parm3);

        public TRet FourParamCall<TIn1, TIn2, TIn3, TIn4, TRet>(TIn1 parm1, TIn2 parm2, TIn3 parm3, TIn4 parm4) =>
            MakeHttpCall<TRet>(parm1, parm2, parm3, parm4);

        #endregion

        private TRet MakeHttpCall<TRet>(params object[] parms) =>
            HttpCallFactory.MakeCall<TRet>(_endpointConfig, _settings, parms);

        #region Helper Methods                        

        #endregion
    }
}
