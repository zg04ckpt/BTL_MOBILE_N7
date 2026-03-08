# Vue 3 + TypeScript + Vite Project
## Cấu trúc dự án

```
web/
├── public/              # Static assets
├── src/
│   ├── assets/         # CSS, images, fonts
│   ├── components/     # Vue components
│   ├── views/          # Page components
│   ├── router/         # Vue Router configuration
│   ├── stores/         # Pinia stores
│   ├── services/       # API services
│   ├── types/          # TypeScript types/interfaces
│   ├── utils/          # Utility functions
│   ├── App.vue         # Root component
│   └── main.ts         # Application entry point
├── index.html          # HTML template
├── vite.config.ts      # Vite configuration
├── tsconfig.json       # TypeScript configuration
└── package.json        # Dependencies
```

## Cài đặt

```bash
npm install
```

## Chạy development server

```bash
npm run dev
```

Ứng dụng sẽ chạy tại: http://localhost:3000

## Build cho production

```bash
npm run build
```