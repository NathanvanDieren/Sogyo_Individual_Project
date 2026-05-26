declare module '*.vue' {
    import type { DefineComponent } from 'vue'
    // eslint-disable-next-line @typescript-eslint/no-explicit-any, @typescript-eslint/no-empty-object-type
    const component: DefineComponent<{}, {}, any>
    export default component
}

declare module '*.css' {
    const content: Record<string, string>;
    export default content;
}