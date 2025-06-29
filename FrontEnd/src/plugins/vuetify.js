// Styles
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'
import { aliases, md } from 'vuetify/iconsets/md'

// Vuetify
import { createVuetify } from 'vuetify'
import { createI18n } from 'vue-i18n';
import { VDateInput } from 'vuetify/labs/VDateInput'
import { es, en } from 'vuetify/locale';

const messages = {
  es: {
    ...es,
  },
  en: {
    ...en,
  },
};

const myCustomLightTheme = {
  dark: false,
  colors: {
    background: '#FFFFFF',
    surface: '#FFFFFF',
    'surface-bright': '#FFFFFF',
    'surface-light': '#EEEEEE',
    'surface-variant': '#424242',
    'on-surface-variant': '#EEEEEE',
    primary: '#1867C0',
    'primary-darken-1': '#1F5592',
    secondary: '#48A9A6',
    'secondary-darken-1': '#018786',
    error: '#B00020',
    info: '#2196F3',
    success: '#4CAF50',
    warning: '#FB8C00',
  },
  variables: {
    'border-color': '#000000',
    'border-opacity': 0.12,
    'high-emphasis-opacity': 0.87,
    'medium-emphasis-opacity': 0.60,
    'disabled-opacity': 0.38,
    'idle-opacity': 0.04,
    'hover-opacity': 0.04,
    'focus-opacity': 0.12,
    'selected-opacity': 0.08,
    'activated-opacity': 0.12,
    'pressed-opacity': 0.12,
    'dragged-opacity': 0.08,
    'theme-kbd': '#212529',
    'theme-on-kbd': '#FFFFFF',
    'theme-code': '#F5F5F5',
    'theme-on-code': '#000000',
  }
};

const i18n = createI18n({
  legacy: false,
  locale: 'es',
  messages,
});

const vuetify = createVuetify({
  components: {
    VDateInput,
  },
  locale: {
    locale: 'es',
    messages,
  },
  icons: {
    defaultSet: 'md',
    aliases,
    sets: {
      md,
    },
  },
  theme: {
    defaultTheme: 'myCustomLightTheme',
    themes: {
      myCustomLightTheme,
    },
  },
})
export { vuetify, i18n };