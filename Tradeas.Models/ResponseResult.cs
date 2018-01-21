namespace Tradeas.Models
{
    public class ResponseResult<T> : Result<T>
    {
        public string Reason { get; set; }
        public string StatusCode { get; set; }

        public ResponseResult()
        {}

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.ResponseResult`1"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.ResponseResult`1"/>.</returns>
        public override string ToString()
        {
            string baseString = base.ToString();
            return string.Format("[ResponseResult: Reason={0}, ErrorCode={1}, {2}]", Reason, StatusCode, baseString);
        }
    }
}
