import {themes as prismThemes} from 'prism-react-renderer';
import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';

const config: Config = {
  title: 'Fetcharr',
  tagline: 'Documentation for Fetcharr',

  url: 'https://fetcharr.github.io',
  baseUrl: '/',
  favicon: 'img/favicon.ico',

  organizationName: 'fetcharr',
  projectName: 'fetcharr',

  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',

  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },

  presets: [
    [
      'classic',
      {
        pages: false,
        docs: {
          routeBasePath: "/",
          sidebarPath: './sidebars.ts',
          editUrl: 'https://github.com/fetcharr/fetcharr/tree/main/docs/',
        },
        blog: false,
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    navbar: {
      title: 'Fetcharr',
      logo: {
        alt: 'Fetcharr Logo',
        src: 'img/logo.svg',
      },
      items: [
        {
          href: 'https://github.com/fetcharr/fetcharr',
          position: 'right',
          className: "header-github-link",
          "aria-label": "Github repository"
        },
      ],
    },
    prism: {
      theme: prismThemes.palenight,
      /**
       * Slightly modified Dracula theme, optimized for YAML, since it has no highlights by default.
       */
      darkTheme: {
        plain: {
          // Both values changes from Dracula default
          color: '#c6d0f5',
          backgroundColor: '#232634'
        },
        styles: [
          /* Start of custom additions to Dracula */
          {
            types: ["key"],
            style: {
              color: "#FF79C6",
            },
          },
          {
            types: ["important"],
            style: {
              color: "#50FA7B",
            },
          },
          /* End of custom additions to Dracula */
          {
            types: ["prolog", "constant", "builtin"],
            style: {
              color: "rgb(189, 147, 249)",
            },
          },
          {
            types: ["inserted", "function"],
            style: {
              color: "rgb(80, 250, 123)",
            },
          },
          {
            types: ["deleted"],
            style: {
              color: "rgb(255, 85, 85)",
            },
          },
          {
            types: ["changed"],
            style: {
              color: "rgb(255, 184, 108)",
            },
          },
          {
            types: ["punctuation", "symbol"],
            style: {
              color: "rgb(248, 248, 242)",
            },
          },
          {
            types: ["string", "char", "tag", "selector"],
            style: {
              color: "rgb(255, 121, 198)",
            },
          },
          {
            types: ["keyword", "variable"],
            style: {
              color: "rgb(189, 147, 249)",
              fontStyle: "italic",
            },
          },
          {
            types: ["comment"],
            style: {
              color: "rgb(98, 114, 164)",
            },
          },
          {
            types: ["attr-name"],
            style: {
              color: "rgb(241, 250, 140)",
            },
          },
        ],
      },
      additionalLanguages: ['bash']
    },
    colorMode: {
      defaultMode: 'dark',
      disableSwitch: true
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
