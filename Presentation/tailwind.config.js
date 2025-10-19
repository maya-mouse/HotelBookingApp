/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    // !!! КРИТИЧНО: Додати шляхи до ваших Razor Pages
    "./Pages/**/*.cshtml",
    "./Views/**/*.cshtml",
    "./wwwroot/js/**/*.js", 
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
