/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      colors: {
        brand: {
          50:  '#f0f9f4',
          100: '#dcf2e3',
          200: '#b9e4ca',
          300: '#8dd4aa',
          400: '#5cbc85',
          500: '#3b9a5f',
          600: '#2e7d4d',
          700: '#1f5a37',
          800: '#154025',
          900: '#0e2e1c',
        },
      },
    },
  },
  plugins: [],
};
