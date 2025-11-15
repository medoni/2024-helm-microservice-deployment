/**
 * Generates a UUID v4 compatible string
 * Works in both HTTP and HTTPS contexts
 * @returns {string} UUID v4 string
 */
export function generateUUID() {
  // Try to use crypto.randomUUID if available (HTTPS context)
  if (typeof crypto !== 'undefined' && crypto.randomUUID) {
    return crypto.randomUUID();
  }

  // Fallback implementation for HTTP contexts
  // Using Math.random() - sufficient for client-side IDs
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
    const r = Math.random() * 16 | 0;
    const v = c === 'x' ? r : (r & 0x3 | 0x8);
    return v.toString(16);
  });
}
