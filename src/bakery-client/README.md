# Bakery Client - Vue.js Frontend

Vue.js 3 frontend application for The Bakery API.

## Recommended IDE Setup

[VS Code](https://code.visualstudio.com/) + [Vue (Official)](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Recommended Browser Setup

- Chromium-based browsers (Chrome, Edge, Brave, etc.):
  - [Vue.js devtools](https://chromewebstore.google.com/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd) 
  - [Turn on Custom Object Formatter in Chrome DevTools](http://bit.ly/object-formatters)
- Firefox:
  - [Vue.js devtools](https://addons.mozilla.org/en-US/firefox/addon/vue-js-devtools/)
  - [Turn on Custom Object Formatter in Firefox DevTools](https://fxdx.dev/firefox-devtools-custom-object-formatters/)

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vite.dev/config/).

## Features

- Browse bakery items by category
- Add items to shopping cart
- Manage cart (update quantities, remove items)
- Checkout and place orders
- Responsive design

## Prerequisites

- Node.js 18+ and npm
- Bakery API running (default: https://localhost:7165)

## Project Setup

1. **Install dependencies**:
   ```sh
   npm install
   ```

2. **Configure API URL** (if needed):
   Update the API base URL in `src/services/bakeryService.ts` if your API runs on a different port.

3. **Run development server**:
   ```sh
   npm run dev
   ```

4. **Access the application**:
   Navigate to the URL shown in the console (typically http://localhost:5173)

## Build for Production

```sh
npm run build
```

## Technologies Used

- Vue.js 3 (Composition API with TypeScript)
- Pinia (State Management)
- Vue Router
- Vite (Build Tool)
