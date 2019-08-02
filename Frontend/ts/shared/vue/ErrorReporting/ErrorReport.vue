<template v-if="errors.length > 0">
  <div class="error-report">
    <div v-for="(group, name) in GroupedErrors" :key="name">
      <SchemaExceptionGroup :errors="group" v-if="name == 'SchemaError'" />
      <MissingArtifactGroup :errors="group" v-if="name == 'MissingArtifactError'" />
      <ErrorGroup :errors="group" :title="name" v-else />
    </div>
  </div>
</template>


<script lang="ts">
import Vue from "vue";
import { Component, Prop } from "vue-property-decorator";
import ErrorGroup from "shared/vue/ErrorReporting/ErrorGroup.vue";
import SchemaExceptionGroup from "shared/vue/ErrorReporting/SchemaExceptionGroup.vue";
import MissingArtifactGroup from "shared/vue/ErrorReporting/MissingArtifactGroup.vue";

@Component({ components: { ErrorGroup, SchemaExceptionGroup, MissingArtifactGroup } })
export default class ErrorReport extends Vue {
  @Prop({ type: Array, required: false, default: () => [] }) errors: any[];

  groupBy<T extends any, K extends keyof T>(
    array: T[],
    key: K
  ): Record<T[K], T[]> {
    return array.reduce(
      (objectsByKeyValue, obj) => {
        const value = obj[key];
        objectsByKeyValue[value] = (objectsByKeyValue[value] || []).concat(obj);
        return objectsByKeyValue;
      },
      {} as Record<T[K], T[]>
    );
  }

  get GroupedErrors() {
    return this.groupBy<any, string>(this.errors, "name");
  }

  public static ReportErrors(
    target: any,
    propertyKey: string | symbol,
    descriptor: PropertyDescriptor
  ) {
    var originalMethod = descriptor.value;
    descriptor.value = function() {
      var context = this;
      var args = arguments;
      var promise: Promise<void> = originalMethod.apply(context, args);
      promise.catch(e => {
        context.errors = e.response.data.errors;
      });
    };
    return descriptor;
  }
}
</script>

<style lang="scss" scoped>
@import "~@syncfusion/ej2-vue-inplace-editor/styles/bootstrap.scss";
</style>

