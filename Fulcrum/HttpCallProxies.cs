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

        public TRet FiveParamCall<TIn1, TIn2, TIn3, TIn4, TIn5, TRet>(TIn1 parm1, TIn2 parm2, TIn3 parm3, TIn4 parm4, TIn5 parm5) =>
            MakeHttpCall<TRet>(parm1, parm2, parm3, parm4, parm5);

        public TRet SixParamCall<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TRet>(TIn1 parm1, TIn2 parm2, TIn3 parm3, TIn4 parm4, TIn5 parm5, TIn6 parm6) =>
            MakeHttpCall<TRet>(parm1, parm2, parm3, parm4, parm5, parm6);

        public TRet SevenParamCall<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TRet>(TIn1 parm1, TIn2 parm2, TIn3 parm3, TIn4 parm4, TIn5 parm5, TIn6 parm6, TIn7 parm7) =>
            MakeHttpCall<TRet>(parm1, parm2, parm3, parm4, parm5, parm6, parm7);

        #endregion

        private TRet MakeHttpCall<TRet>(params object[] parms) =>
            HttpCallFactory.MakeCall<TRet>(_endpointConfig, _settings, parms);

        #region Helper Methods                        

        #endregion
    }
}
