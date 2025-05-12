export const WasmEnableThreads = false;
export const gitHash: string = '';
export const exceptions: () => Promise<boolean> = () => Promise.resolve(false);
export const simd: () => Promise<boolean> = () => Promise.resolve(true);
export const BuildConfiguration: string = "Debug";