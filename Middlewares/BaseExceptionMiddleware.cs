using cloud.Exceptions;

namespace cloud.Middlewares {
    public class BaseExceptionMiddleware {
        private readonly RequestDelegate next;

        public BaseExceptionMiddleware(RequestDelegate next) {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await next(context);
            } catch (NotFoundException ex) {
                await ExceptionHandler.HandleExceptionAsync(context, 404, ex.Message);
            } catch (FormatException ex) {
                await ExceptionHandler.HandleExceptionAsync(context, 400, ex.Message);
            } catch (InvalidActionException ex) {
                await ExceptionHandler.HandleExceptionAsync(context, 400, ex.Message);
            } catch (UnauthorizedAccessException ex) {
                await ExceptionHandler.HandleExceptionAsync(context, 401, ex.Message);
            } catch (AccessDeniedException ex) {
                await ExceptionHandler.HandleExceptionAsync(context, 403, ex.Message);
            } catch (Exception ex) {
                await ExceptionHandler.HandleExceptionAsync(context, 500, ex.Message);
            }
        }
    }
}