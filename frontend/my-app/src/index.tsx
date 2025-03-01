import React from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App';
import { ThemeProvider } from './ThemeContext';
import { GoogleOAuthProvider } from '@react-oauth/google';
const domNode = document.getElementById('root') as HTMLElement;
const root = createRoot(domNode);
const clientId = '76827373375-6un7nf2edfhjr8b28ig8s64jt64mmk5g.apps.googleusercontent.com';
console.log(clientId);
root.render(
  <React.StrictMode>
    <GoogleOAuthProvider clientId={clientId}>
      <ThemeProvider>
        <App />
      </ThemeProvider>
    </GoogleOAuthProvider>
  </React.StrictMode>
);
