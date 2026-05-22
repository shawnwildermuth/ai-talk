<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <div class="flex flex-col items-center justify-center py-24 text-center space-y-6">
    <span class="text-8xl">🥨</span>
    <h1 class="text-4xl font-bold">404 – Pagina Niet Gevonden</h1>
    <p class="text-base-content/60 max-w-md">
      Sorry, deze pagina bestaat niet. Misschien is het menu veranderd?
    </p>
    <RouterLink to="/" class="btn btn-primary">← Terug naar Dashboard</RouterLink>
  </div>
</template>
