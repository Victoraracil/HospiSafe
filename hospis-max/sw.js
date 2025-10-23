const CACHE_NAME = 'hospisafe-v1.2.0';
const urlsToCache = [
  '/',
  '/index.html',
  'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css',
  'https://cdn.jsdelivr.net/npm/chart.js',
  'https://cdn.jsdelivr.net/npm/jsqr@1.4.0/dist/jsQR.js'
];

self.addEventListener('install', function(event) {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(function(cache) {
        console.log('Opened cache');
        return cache.addAll(urlsToCache);
      })
  );
});

self.addEventListener('fetch', function(event) {
  event.respondWith(
    caches.match(event.request)
      .then(function(response) {
        // Cache hit - return response
        if (response) {
          return response;
        }
        return fetch(event.request);
      }
    )
  );
});

self.addEventListener('activate', function(event) {
  event.waitUntil(
    caches.keys().then(function(cacheNames) {
      return Promise.all(
        cacheNames.map(function(cacheName) {
          if (cacheName !== CACHE_NAME) {
            return caches.delete(cacheName);
          }
        })
      );
    })
  );
});

// Push notification handling
self.addEventListener('push', function(event) {
  const options = {
    body: event.data ? event.data.text() : 'Nueva notificación de HospiSafe',
    icon: '/icon-192.png',
    badge: '/badge-72.png',
    vibrate: [100, 50, 100],
    data: {
      dateOfArrival: Date.now(),
      primaryKey: 1
    },
    actions: [
      {
        action: 'explore',
        title: 'Ver detalles',
        icon: '/icon-explore.png'
      },
      {
        action: 'close',
        title: 'Cerrar',
        icon: '/icon-close.png'
      }
    ]
  };

  event.waitUntil(
    self.registration.showNotification('HospiSafe', options)
  );
});

// Background sync for offline actions
self.addEventListener('sync', function(event) {
  if (event.tag == 'background-sync') {
    event.waitUntil(syncOfflineData());
  }
});

function syncOfflineData() {
  // Sync offline patient data, appointments, etc.
  return new Promise((resolve) => {
    console.log('Syncing offline data...');
    // Simulate sync
    setTimeout(resolve, 1000);
  });
}

// Install prompt handling
let deferredPrompt;

self.addEventListener('beforeinstallprompt', (e) => {
  e.preventDefault();
  deferredPrompt = e;
  
  // Send message to main thread
  self.clients.matchAll().then(clients => {
    clients.forEach(client => {
      client.postMessage({
        type: 'PWA_INSTALLABLE',
        message: 'App can be installed'
      });
    });
  });
});

// Handle offline/online status
self.addEventListener('message', event => {
  if (event.data && event.data.type === 'SKIP_WAITING') {
    self.skipWaiting();
  }
});

// Emergency offline functionality
self.addEventListener('fetch', function(event) {
  // Handle emergency requests when offline
  if (event.request.url.includes('/api/emergency') && !navigator.onLine) {
    event.respondWith(
      new Response(
        JSON.stringify({
          message: 'Emergency request queued for when online',
          timestamp: new Date().toISOString(),
          offline: true
        }),
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      )
    );
  }
});

// Performance monitoring
self.addEventListener('fetch', function(event) {
  const start = performance.now();
  
  event.respondWith(
    caches.match(event.request)
      .then(function(response) {
        const end = performance.now();
        const responseTime = end - start;
        
        // Log slow requests
        if (responseTime > 1000) {
          console.warn(`Slow request: ${event.request.url} took ${responseTime}ms`);
        }
        
        return response || fetch(event.request);
      })
  );
});