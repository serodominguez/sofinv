<template>
  <HelloWorld />
</template>

<script>
import { defineComponent } from 'vue';

// Components
import HelloWorld from '../components/HelloWorld.vue';

export default defineComponent({
  name: 'Dashboardview',

  components: {
    HelloWorld,
  },
});
</script>